"""Collect a bounded Steam review sample; full source text stays in the temp cache.

Run with Python 3.13; pass --output with a new directory for a later sample.
Public artifacts contain metadata and at most 18 words of excerpt per review.
"""
import argparse
import concurrent.futures
import csv
import datetime as dt
import hashlib
import json
import os
from pathlib import Path
import re
import time
import urllib.parse
import urllib.request

OUT = Path(__file__).resolve().parent
CACHE = Path(os.environ.get("TEMP", ".")) / "game-test-steam-research-20260909"
GAMES = {"meltopia": 3601800, "super-motherload": 269110, "digging-a-hole": 3244220}


def fetch(url):
    for attempt in range(3):
        try:
            request = urllib.request.Request(url, headers={"User-Agent": "SteamReviewResearch/1.0"})
            with urllib.request.urlopen(request, timeout=45) as response:
                return json.load(response)
        except Exception:
            if attempt == 2:
                raise
            time.sleep(2 ** attempt)


def collect(game, appid, excluded, negative_target, positive_target):
    records, requests = {}, []
    now = dt.datetime.now(dt.timezone.utc).isoformat()
    for polarity, ordering, target in [("negative", "all", min(50, negative_target)), ("negative", "recent", negative_target), ("positive", "all", positive_target), ("positive", "recent", positive_target)]:
        if sum(r["voted_up"] == (polarity == "positive") for r in records.values()) >= target:
            continue
        cursor, seen_cursors = "*", set()
        for page in range(20):
            params = dict(json=1, filter=ordering, language="english", review_type=polarity,
                          purchase_type="all", num_per_page=50 if polarity == "negative" or excluded else 25,
                          day_range=365, filter_offtopic_activity=1, cursor=cursor)
            url = f"https://store.steampowered.com/appreviews/{appid}?" + urllib.parse.urlencode(params)
            data = fetch(url)
            if data.get("success") != 1:
                raise RuntimeError(f"Steam review request failed: {game}")
            requests.append(dict(url=url, fetched_at=dt.datetime.now(dt.timezone.utc).isoformat(),
                                 returned=len(data.get("reviews", [])), query_summary=data.get("query_summary")))
            (CACHE / f"{game}-{polarity}-{ordering}-{page}.json").write_text(json.dumps(data, ensure_ascii=False), encoding="utf-8")
            for review in data.get("reviews", []):
                key = review["recommendationid"]
                if key in excluded:
                    continue
                if key in records:
                    if ordering not in records[key]["sample_streams"]:
                        records[key]["sample_streams"].append(ordering)
                    continue
                count = sum(r["voted_up"] == (polarity == "positive") for r in records.values())
                if count >= target:
                    continue
                review.update(game=game, appid=appid, sample_streams=[ordering], fetched_at=now)
                records[key] = review
            count = sum(r["voted_up"] == (polarity == "positive") for r in records.values())
            if count >= target or not data.get("reviews"):
                break
            cursor = data.get("cursor")
            if not cursor or cursor in seen_cursors or ordering == "all":
                break
            seen_cursors.add(cursor)
            time.sleep(0.3)
    return list(records.values()), dict(game=game, appid=appid, requests=requests)


def main():
    global OUT, CACHE
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--output", type=Path, default=OUT, help="Use a new directory to collect another dated sample.")
    parser.add_argument("--exclude", type=Path, help="Previous audit/review-index.csv containing every collected recommendation_id.")
    parser.add_argument("--negative-target", type=int, default=100)
    parser.add_argument("--positive-target", type=int, default=25)
    args = parser.parse_args()
    if not 1 <= args.negative_target <= 200 or not 1 <= args.positive_target <= 100:
        parser.error("Targets must be 1-200 negative and 1-100 positive per game.")
    excluded = set()
    if args.exclude:
        with args.exclude.open(encoding="utf-8-sig", newline="") as f:
            excluded = {row["recommendation_id"] for row in csv.DictReader(f)}
    OUT = args.output.resolve()
    OUT.mkdir(parents=True, exist_ok=True)
    if (OUT / "reviews.csv").exists() or (OUT / "manifest.json").exists():
        parser.error("Evidence already exists. Use --output with a new directory; collection never overwrites an audit.")
    CACHE = Path(os.environ.get("TEMP", ".")) / ("game-test-steam-research-" + dt.datetime.now(dt.timezone.utc).strftime("%Y%m%dT%H%M%S%fZ"))
    CACHE.mkdir(parents=True, exist_ok=True)
    corpus, manifests = [], []
    with concurrent.futures.ThreadPoolExecutor(max_workers=3) as pool:
        futures = [pool.submit(collect, game, appid, excluded, args.negative_target, args.positive_target) for game, appid in GAMES.items()]
        for future in futures:
            records, manifest = future.result()
            corpus.extend(records)
            manifests.append(manifest)
    (CACHE / "corpus-full.json").write_text(json.dumps(corpus, ensure_ascii=False, indent=2), encoding="utf-8")
    rows = []
    for r in corpus:
        clean = re.sub(r"\[[^\]]*\]", "", r["review"])
        text = " ".join(clean.split())
        author = r["author"]
        rows.append(dict(game=r["game"], appid=r["appid"], recommendation_id=r["recommendationid"],
                         polarity="positive" if r["voted_up"] else "negative", language=r["language"],
                         sample_streams="|".join(r["sample_streams"]),
                         created_utc=dt.datetime.fromtimestamp(r["timestamp_created"], dt.timezone.utc).isoformat(),
                         updated_utc=dt.datetime.fromtimestamp(r["timestamp_updated"], dt.timezone.utc).isoformat(),
                         playtime_at_review_minutes=author.get("playtime_at_review", ""),
                         helpful_votes=r["votes_up"], steam_purchase=r["steam_purchase"],
                         received_for_free=r["received_for_free"], refunded=r.get("refunded", ""),
                         early_access=r["written_during_early_access"], primarily_steam_deck=r.get("primarily_steam_deck", ""),
                         source_url=f'https://steamcommunity.com/profiles/{author["steamid"]}/recommended/{r["appid"]}/',
                         review_text_sha256=hashlib.sha256(r["review"].encode("utf-8")).hexdigest(),
                         word_count=len(text.split()), excerpt=" ".join(text.split()[:18]), fetched_utc=r["fetched_at"]))
    if not rows:
        raise RuntimeError("No unseen reviews returned; no reading dataset was written. Inspect the collection cache.")
    (OUT / "audit").mkdir(exist_ok=True)
    reading = [dict(recommendation_id=r["recommendation_id"], game=r["game"], verdict=r["polarity"],
                    excerpt=r["excerpt"], source_url=r["source_url"]) for r in rows]
    metadata = [dict(r, screening_status="not-screened") for r in rows]
    outputs = [(OUT / "reviews.csv", reading, ["recommendation_id", "game", "verdict", "excerpt", "source_url"]),
               (OUT / "audit/review-index.csv", metadata,
                ["recommendation_id", "sample_streams", "created_utc", "updated_utc", "fetched_utc",
                 "word_count", "review_text_sha256", "screening_status"])]
    for path, data_rows, fields in outputs:
        with path.open("w", encoding="utf-8-sig", newline="") as f:
            writer = csv.DictWriter(f, fieldnames=fields, extrasaction="ignore")
            writer.writeheader()
            writer.writerows({k: ("'" + v if isinstance(v, str) and v.startswith(("=", "+", "-", "@", "\t", "\r")) else v)
                             for k, v in row.items()} for row in data_rows)
    summary = {game: {pol: sum(r["game"] == game and r["polarity"] == pol for r in rows)
                      for pol in ["negative", "positive"]} for game in GAMES}
    manifest = dict(fetched_at=dt.datetime.now(dt.timezone.utc).isoformat(), summary=summary,
                    protocol=f"Up to {min(50, args.negative_target)} helpful-window negatives, recent top-up to {args.negative_target} unique negatives; {args.positive_target} helpful-window positives with recent fallback per game; English-labeled; all purchase types; default off-topic exclusion",
                    excluded_count=len(excluded), excluded_ids_sha256=hashlib.sha256("\n".join(sorted(excluded)).encode("utf-8")).hexdigest(),
                    source_documentation="https://partner.steamgames.com/doc/store/getreviews?l=english", games=manifests)
    (OUT / "manifest.json").write_text(json.dumps(manifest, ensure_ascii=False, indent=2), encoding="utf-8")
    print(json.dumps(dict(summary=summary, total=len(rows), full_text_cache=str(CACHE / "corpus-full.json")), indent=2))


if __name__ == "__main__":
    main()

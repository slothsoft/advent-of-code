using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/5">Day 5: If You Give A Seed A Fertilizer</a>: What is the lowest location number that corresponds to any of the initial seed numbers?
/// </summary>
public class SeedFertilizer {
    public record Almanac {
        public AlmanacMap[] AlmanacMaps { get; } = new AlmanacMap[MAX_ALMANAC_MAPS];

        public AlmanacMap SeedToSoilMap {
            get => AlmanacMaps[SEED_TO_SOIL]; 
            set => AlmanacMaps[SEED_TO_SOIL] = value;
        }

        public AlmanacMap SoilToFertilizerMap {
            get => AlmanacMaps[SOIL_TO_FERTILIZER];
            set => AlmanacMaps[SOIL_TO_FERTILIZER] = value;
        }

        public AlmanacMap FertilizerToWaterMap {
            get => AlmanacMaps[FERTILIZER_TO_WATER];
            set => AlmanacMaps[FERTILIZER_TO_WATER] = value;
        }

        public AlmanacMap WaterToLightMap {
            get => AlmanacMaps[WATER_TO_LIGHT];
            set => AlmanacMaps[WATER_TO_LIGHT] = value;
        }

        public AlmanacMap LightToTemperatureMap {
            get => AlmanacMaps[LIGHT_TO_TEMPERATURE];
            set => AlmanacMaps[LIGHT_TO_TEMPERATURE] = value;
        }

        public AlmanacMap TemperatureToHumidityMap {
            get => AlmanacMaps[TEMPERATURE_TO_HUMIDITY];
            set => AlmanacMaps[TEMPERATURE_TO_HUMIDITY] = value;
        }

        public AlmanacMap HumidityToLocationMap {
            get => AlmanacMaps[HUMIDITY_TO_LOCATION];
            set => AlmanacMaps[HUMIDITY_TO_LOCATION] = value;
        }

        public long GetSeedLocation(long seed) {
            var result = seed;
            foreach (var almanacMap in AlmanacMaps) {
                result = almanacMap[result];
            }
            
            return result;
        }
    }

    public record AlmanacMap {
        private IList<AlmanacMapRange> _ranges = new List<AlmanacMapRange>();

        public long this[long key] {
            get {
                var range = _ranges.SingleOrDefault(r => r.Contains(key));
                return range?[key] ?? key;
            }
        }

        public void AddRange(long[] range) {
            _ranges.Add(new AlmanacMapRange(range));
        }
    }

    internal record AlmanacMapRange {
        internal AlmanacMapRange(long[] destinationSourceLength) {
            DestinationRange = new Range(destinationSourceLength[0], destinationSourceLength[0] + destinationSourceLength[2]);
            SourceRange = new Range(destinationSourceLength[1], destinationSourceLength[1] + destinationSourceLength[2]);
        }

        public Range DestinationRange { get; }
        public Range SourceRange { get; }

        public long this[long key] {
            get {
                return key - SourceRange.From + DestinationRange.From;
            }
        }

        public bool Contains(long value) {
            return SourceRange.Contains(value);
        }

        public Range? Intersects(long rangeFrom, long rangeTo) {
            var fromMax = Math.Max(rangeFrom, DestinationRange.From);
            var toMin = Math.Min(rangeTo, DestinationRange.To);

            if (fromMax >= toMin) {
                return null;
            }

            return new Range(fromMax, toMin);
        }
        
        public override string ToString() => $"AlmanacMapRange: ({SourceRange}) -> ({DestinationRange})";
    }

    internal struct Range {
        public Range(long from, long to) {
            From = from;
            To = to;
        }

        public long From { get; }
        public long To { get; }

        public bool Contains(long value) {
            return value.CompareTo(From) >= 0 && value.CompareTo(To) < 0;
        }

        public Range? CalculateIntersection(long rangeFrom, long rangeTo) {
            var fromMax = Math.Max(rangeFrom, From);
            var toMin = Math.Min(rangeTo, To);

            if (fromMax >= toMin) {
                return null;
            }

            return new Range(fromMax, toMin);
        }

        public override string ToString() => $"Range: {From} -> {To}";
    }

    private const long SEED_TO_SOIL = 0;
    private const long SOIL_TO_FERTILIZER = 1;
    private const long FERTILIZER_TO_WATER = 2;
    private const long WATER_TO_LIGHT = 3;
    private const long LIGHT_TO_TEMPERATURE = 4;
    private const long TEMPERATURE_TO_HUMIDITY = 5;
    private const long HUMIDITY_TO_LOCATION = 6;
    private const long MAX_ALMANAC_MAPS = 7;

    private Almanac _almanac;

    public SeedFertilizer(IEnumerable<string> input) {
        (_almanac, Seeds) = ParseAlmanac(input);
    }
    
    public long[] Seeds { get; set; }

    public static (Almanac, long[]) ParseAlmanac(IEnumerable<string> input) {
        var inputEnumerator = input.GetEnumerator();
        try {
            inputEnumerator.MoveNext();
            var seeds = inputEnumerator.Current.Split(':')[1].ParseLongArray();

            inputEnumerator.MoveNext();
            var result = new Almanac();
            var currentMap = -1;

            do {
                if (inputEnumerator.Current.Length == 0) {
                    // we ignore empty lines
                    continue;
                }

                if (inputEnumerator.Current.Contains(':')) {
                    // next map starts!
                    currentMap++;
                    continue;
                }

                if (currentMap >= 0) {
                    // we have an map so this must be a range
                    result.AlmanacMaps[currentMap] ??= new AlmanacMap();
                    result.AlmanacMaps[currentMap].AddRange(inputEnumerator.Current.ParseLongArray());
                }
            } while (inputEnumerator.MoveNext());

            return (result, seeds);
        } finally {
            inputEnumerator.Dispose();
        }
    }

    public long GetLowestSeedLocation() {
        var result = long.MaxValue;
        for (var i = 0; i < Seeds.Length; i++) {
            result = Math.Min(result, _almanac.GetSeedLocation(Seeds[i]));
        }

        return result;
    }
}
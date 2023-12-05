using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/5">Day 5: If You Give A Seed A Fertilizer</a>: What is the lowest location number that corresponds to any of the initial seed numbers?
/// </summary>
public class SeedFertilizer {
    public record Almanac() {
        public AlmanacMap[] AlmanacMaps { get; } = new AlmanacMap[MAX_ALMANAC_MAPS];
        public AlmanacMap SeedToSoilMap { get => AlmanacMaps[SEED_TO_SOIL]; set => AlmanacMaps[SEED_TO_SOIL] = value; }

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
            return AlmanacMaps.Aggregate(seed, (current, almanacMap) => almanacMap[current]);
        }

        public long GetMinSeedLocation(long fromSeed, long toSeed) {
            var mapFunctions = AlmanacMaps.Select(m => m.FetchFunction(fromSeed, toSeed)).ToArray();
            var result = long.MaxValue;
          
            for (var seed = fromSeed; seed < toSeed; seed++) {
                result = Math.Min(result, mapFunctions.Aggregate(seed, (current, functions) => functions[current]));
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
        
        public IDictionary<(long From, long To), Func<long, long>> FetchFunction(long fromSeed, long toSeed) {
            var rangesToFunction = new Dictionary<(long From, long To), Func<long, long>> {
                { (fromSeed, toSeed), seed => seed }
            };

            foreach (var rangeWithFunction in _ranges) {
                foreach (var range in rangesToFunction.Keys.ToArray()) {
                    var intersection = rangeWithFunction.Intersects(range.From, range.To);
                    if (intersection != null) {
                        var function = rangesToFunction[range];
                        rangesToFunction.Remove(range);
                            
                        if (range.From < intersection.Value.From) {
                            rangesToFunction.Add((range.From, intersection.Value.From), function);
                        }
                        rangesToFunction.Add(intersection.Value, input => rangeWithFunction[input]);
                        
                        if (range.To > intersection.Value.To) {
                            rangesToFunction.Add((intersection.Value.To, range.To), function);
                        }
                    }
                }
            }
            
            return rangesToFunction;
        }
    }

    private record AlmanacMapRange(long[] DestinationSourceLength) {
        public long this[long key] {
            get {
                return key - DestinationSourceLength[1] + DestinationSourceLength[0];
            }
        }

        public bool Contains(long value) {
            return value >= DestinationSourceLength[1] &&
                   value < DestinationSourceLength[1] + DestinationSourceLength[2];
        }

        public (long From, long To)? Intersects(long rangeFrom, long rangeTo) {
            var fromMax = Math.Max(rangeFrom, DestinationSourceLength[1]);
            var toMin = Math.Min(rangeTo, DestinationSourceLength[1] + DestinationSourceLength[2]);

            if (fromMax >= toMin) {
                return null;
            }
            
            return (fromMax, toMin);
        }
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
    private long[] _seeds;

    public SeedFertilizer(IEnumerable<string> input) {
        (_almanac, _seeds) = ParseAlmanac(input);
    }

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
        for (var i = 0; i < _seeds.Length; i++) {
            result = Math.Min(result, _almanac.GetSeedLocation(_seeds[i]));
        }

        return result;
    }

    public long GetLowestSeedLocationForRange() {
        var result = long.MaxValue;
        for (var i = 0; i < _seeds.Length; i+=2) {
            result = Math.Min(result, _almanac.GetMinSeedLocation(_seeds[i], _seeds[i] + _seeds[i + 1]));
        }

        return result;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SVO.Console {
    class CsvReader : IEnumerable<Dictionary<string, string>> {
        private readonly StreamReader _reader;

        public IReadOnlyList<string> Headers { get; }

        public CsvReader(string path) {            
            _reader = new StreamReader(path);

            Headers = _reader.ReadLine().Split(",");
        }

        public IEnumerator<Dictionary<string, string>> GetEnumerator()
        {
            while (!_reader.EndOfStream) {
                var values = _reader.ReadLine().Split(",");
                yield return Headers.Zip(values)
                    .ToDictionary(p => p.First, p => p.Second);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
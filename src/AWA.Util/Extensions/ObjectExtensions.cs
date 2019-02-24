using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

namespace AWA.Util.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToMd5(this string source)
        {
            using (var md5 = MD5.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(source);
                var temp = md5.ComputeHash(buffer);

                return temp.ToHexString();
            }
        }

        public static string ToSha1(this string source)
        {
            using (var sha1 = SHA1.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(source);
                var temp = sha1.ComputeHash(buffer);

                return temp.ToHexString();
            }
        }

        public static string ToHexString(this byte[] bytes)
        {
            var hexStringTable = new[]
            {
                "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F",
                "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F",
                "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F",
                "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F",
                "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F",
                "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F",
                "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F",
                "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F",
                "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F",
                "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F",
                "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF",
                "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF",
                "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF",
                "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF",
                "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF",
                "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF"
            };

            var result = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                result.Append(hexStringTable[b]);
            }

            return result.ToString();
        }

        public static string ToBase64String(this string source)
        {
            var buffer = Encoding.UTF8.GetBytes(source);
            var result = Convert.ToBase64String(buffer);

            return result;
        }

        public static string FromBase64String(this string source)
        {
            var buffer = Convert.FromBase64String(source);
            var result = Encoding.UTF8.GetString(buffer);

            return result;
        }

        public static bool IsNullOrEmpty(this string source, bool detectWhiteSpace = false)
        {
            return detectWhiteSpace ? string.IsNullOrWhiteSpace(source) : string.IsNullOrEmpty(source);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static IEnumerable<string> SplitBy(this string source, int chunkSize)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException("the string can not be null or empty.", nameof(source));
            }

            if (chunkSize < 1)
            {
                throw new ArgumentException("the split chunk size can not less than one.", nameof(chunkSize));
            }

            for (var i = 0; i < source.Length; i += chunkSize)
            {
                if (chunkSize + i > source.Length)
                    chunkSize = source.Length - i;

                yield return source.Substring(i, chunkSize);
            }
        }

        public static IEnumerable<IEnumerable<T>> Slice<T>(this IEnumerable<T> source, params int[] steps)
        {
            if (!steps.Any(step => step != 0))
            {
                throw new InvalidOperationException("Can't slice a collection with step length 0.");
            }
            return new Slicer<T>(source.GetEnumerator(), steps).Slice();
        }



        /// <summary>
        /// 扩展从string转换为int的方法
        /// </summary>
        /// <param name="source">要转换的string变量</param>
        /// <returns></returns>
        public static int ToInt(this string source)
        {
            int.TryParse(source, out var result);

            return result;
        }
    }
    internal sealed class Slicer<T>
    {
        public Slicer(IEnumerator<T> iterator, int[] steps)
        {
            _iterator = iterator;
            _steps = steps;
            _index = 0;
            _currentStep = 0;
            _isHasNext = true;
        }

        public int Index
        {
            get { return _index; }
        }

        public IEnumerable<IEnumerable<T>> Slice()
        {
            var length = _steps.Length;
            var index = 1;
            var step = 0;

            for (var i = 0; _isHasNext; ++i)
            {
                if (i < length)
                {
                    step = _steps[i];
                    _currentStep = step - 1;
                }

                while (_index < index && _isHasNext)
                {
                    _isHasNext = MoveNext();
                }

                if (_isHasNext)
                {
                    yield return SliceInternal();
                    index += step;
                }
            }
        }

        private IEnumerable<T> SliceInternal()
        {
            if (_currentStep == -1) yield break;
            yield return _iterator.Current;

            for (var count = 0; count < _currentStep && _isHasNext; ++count)
            {
                _isHasNext = MoveNext();

                if (_isHasNext)
                {
                    yield return _iterator.Current;
                }
            }
        }

        private bool MoveNext()
        {
            ++_index;
            return _iterator.MoveNext();
        }

        private readonly IEnumerator<T> _iterator;
        private readonly int[] _steps;
        private volatile bool _isHasNext;
        private volatile int _currentStep;
        private volatile int _index;
    }
}

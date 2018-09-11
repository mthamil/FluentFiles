using AutoFixture;
using BenchmarkDotNet.Attributes;
using FileHelpers;
using FluentFiles.Benchmark.Entities;
using FluentFiles.Benchmark.Mapping;
using FluentFiles.Core;
using FluentFiles.FixedLength.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FluentFiles.Benchmark
{
    [InProcess]
    public class FluentFilesVsFileHelpersRead
    {
        private FileHelperEngine<FixedSampleRecord> _helperEngine;
        private IFlatFileEngine _fluentEngine;

        private string _records;

        [Params(10, 100, 1000, 10000, 100000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            _helperEngine = new FileHelperEngine<FixedSampleRecord>();

            _fluentEngine = new FixedLengthFileEngineFactory()
                .GetEngine(new FixedSampleRecordLayout());

            var records = new StringBuilder(N * 185);
            var random = new Random();
            var fixture = new Fixture().Customize(new RandomStringCustomization(160));
            for (int i = 0; i < N; i++)
            {
                records.AppendLine($"{20000000000L + i}{fixture.Create<string>()}{random.Next(0, 999999).ToString().PadRight(6)}");
            }

            _records = records.ToString();
        }

        [Benchmark(Baseline = true)]
        public IEnumerable<FixedSampleRecord> FileHelpers()
        {
            using (var stream = new StringReader(_records))
            {
                return _helperEngine.ReadStream(stream);
            }
        }

        [Benchmark]
        public IEnumerable<FixedSampleRecord> FluentFiles()
        {
            using (var stream = new StringReader(_records))
            {
                return _fluentEngine.Read<FixedSampleRecord>(stream).ToArray();
            }
        }
    }

    class RandomStringCustomization : ICustomization
    {
        private readonly Random _random = new Random();
        private readonly int _maxLength;

        public RandomStringCustomization(int maxLength)
        {
            _maxLength = maxLength;
        }

        public void Customize(IFixture fixture)
        {
            fixture.RepeatCount = _maxLength;
            fixture.Customize<string>(c => c.FromFactory(() => 
                new string(fixture.CreateMany<char>()
                                  .Take(_random.Next(1, _maxLength + 1))
                                  .ToArray()).PadRight(_maxLength)));
        }
    }
}
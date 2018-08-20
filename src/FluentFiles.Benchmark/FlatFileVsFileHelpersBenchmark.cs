namespace FluentFiles.Benchmark
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using BenchmarkIt;
    using FileHelpers;
    using FluentFiles.Benchmark.Entities;
    using FluentFiles.Benchmark.Mapping;
    using FluentFiles.FixedLength.Implementation;
    using FluentAssertions;
    using Hyper.ComponentModel;
    using Xunit;
    using AutoFixture;

    public class FlatFileVsFileHelpersBenchmark
    {
        private const string FixedFileSample =
            @"20000000109PANIAGUA JOSE                                                                                                                                                   0     
20000000125ACOSTA MARCOS                                                                                                                                                   0     
20000000141GONZALEZ DOMINGO                                                                                                                                                0     
20000000168SENA RAUL                                                                                                                                                       0     
20000000192BITTAR RUSTON MUSA                                                                                                                                              0     
20000000206CORDON SERGIO ALFREDO                                                                                                                                           0     
20000000222CAVAGNARO ERNESTO RODAS GUILLERMO Y RIV                                                                                                                         0     
20000000338BRUCE LUIS                                                                                                                                                      0     
20000000354CHAVEZ DAMIAN JOSE                                                                                                                                              0     
20000000389ZINGARETTI SANTIAGO ENRIQUE ARDUINO                                                                                                                             0     
20000000400PEREYRA ARNOTI LEONARDO ANDRES                                                                                                                                  0     
20000000516VELAZQUEZ ANIBAL ARISTOBU                                                                                                                                       0     
20000000532GONZALEZ DOMINGO                                                                                                                                                0     
20000000613CANALE S A CTA RECAUDADORA                                                                                                                                      0     
20000000656MU#OZ FERNANDEZ ALEJANDRO                                                                                                                                       0     
20000000745FERNANDEZ MONTOYA CHARLES JAIME                                                                                                                                 0     
20000000869MOSCHION DANILO JUAN                                                                                                                                            602290
20000000885CHOQUE RAMON FELIX                                                                                                                                              0     
20000000923AQUINO VILLASANTI NICASIO                                                                                                                                       0     
";

        [Fact]
        [Trait("Category", "Benchmarks")]
        public void ReadOperationShouldBeQuick()
        {
            Benchmark
                .This("FileHelperEngine.ReadStream", () =>
                {
                    var engine = new FileHelperEngine<FixedSampleRecord>();
                    using (var stream = new StringReader(FixedFileSample))
                    {
                        var records = engine.ReadStream(stream);
                        records.Should().HaveCount(19);
                    }
                })
                .Against
                .This("FlatFileEngine.Read", () =>
                {
                    var layout = new FixedSampleRecordLayout();
                    using (var stream = new StringReader(FixedFileSample))
                    {
                        var factory = new FixedLengthFileEngineFactory();
                        var engine = factory.GetEngine(layout);

                        var records = engine.Read<FixedSampleRecord>(stream).ToArray();

                        records.Should().HaveCount(19);
                    }
                })
                .WithWarmup(1000)
                .For(10000)
                .Iterations()
                .PrintComparison();
        }


        [Fact]
        [Trait("Category", "Benchmarks")]
        public void WriteOperationShouldBeQuick()
        {
            var sampleRecords = GetRecords();

            Benchmark
                .This("FileHelperEngine.WriteStream", () =>
                {
                    var engine = new FileHelperEngine<FixedSampleRecord>();
                    using (var stream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(stream))
                    {
                        engine.WriteStream(streamWriter, sampleRecords);
                    }
                })
                .Against
                .This("FlatFileEngine.Write", () =>
                {
                    var layout = new FixedSampleRecordLayout();
                    using (var stream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(stream))
                    {
                        var factory = new FixedLengthFileEngineFactory();
                        var engine = factory.GetEngine(layout);

                        engine.Write(streamWriter, sampleRecords);
                    }
                })
                .WithWarmup(1000)
                .For(10000)
                .Iterations()
                .PrintComparison();
        }


        [Fact]
        [Trait("Category", "Benchmarks")]
        public void BigDataWriteOperationShouldBeQuick()
        {
            var fixture = new Fixture();
            var sampleRecords = fixture.CreateMany<FixedSampleRecord>(100000).ToArray();

            Benchmark
                .This("FileHelperEngine.WriteStream", () =>
                {
                    var engine = new FileHelperEngine<FixedSampleRecord>();
                    using (var stream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(stream))
                    {
                        engine.WriteStream(streamWriter, sampleRecords);
                    }
                })
                .Against
                .This("FlatFileEngine.Write", () =>
                {
                    var layout = new FixedSampleRecordLayout();
                    using (var stream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(stream))
                    {
                        var factory = new FixedLengthFileEngineFactory();
                        var engine = factory.GetEngine(layout);

                        engine.Write(streamWriter, sampleRecords);
                    }
                })
                .WithWarmup(10)
                .For(100)
                .Iterations()
                .PrintComparison();
        }

        [Fact]
        [Trait("Category", "Benchmarks")]
        public void BigDataWriteOperationShouldBeQuickWithReflectionMagic()
        {
            HyperTypeDescriptionProvider.Add(typeof (FixedSampleRecord));

            BigDataWriteOperationShouldBeQuick();
        }

        private static IEnumerable<FixedSampleRecord> GetRecords()
        {
            var engine = new FileHelperEngine<FixedSampleRecord>();
            using (var stream = new StringReader(FixedFileSample))
            {
                var records = engine.ReadStream(stream);
                return records;
            }
        }
    }
}
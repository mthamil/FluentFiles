namespace FluentFiles.Benchmark.Mapping
{
    using FluentFiles.Benchmark.Entities;
    using FluentFiles.FixedLength.Implementation;

    public sealed class FixedSampleRecordLayout : FixedLayout<FixedSampleRecord>
    {
        public FixedSampleRecordLayout()
        {
            this.WithMember(x => x.Cuit, c => c.WithLength(11))
                .WithMember(x => x.Nombre, c => c.WithLength(160))
                .WithMember(x => x.Actividad, c => c.WithLength(6));
        }
    }
}
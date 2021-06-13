using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models.Maps
{
    class ExtratoMap : IEntityTypeConfiguration<Extrato>
    {
        public void Configure(EntityTypeBuilder<Extrato> builder)
        {
            //Common Properties
            builder.HasKey(propertie => propertie.Id);
            builder.Property(propertie => propertie.DataCadastro).HasColumnType("date").IsRequired();
            builder.Property(propertie => propertie.DataAlteracao).HasColumnType("date").IsRequired();
            builder.Property(propertie => propertie.Status).HasColumnType("bit").IsRequired();

            //My Properties
            builder.Property(extrato => extrato.TipoOperacao).HasColumnType("int").IsRequired();
            builder.Property(extrato => extrato.Valor).HasColumnType("varchar(500)").IsRequired();

            //Navigation Properties
            builder.HasOne(extrato => extrato.Conta).WithMany(conta => conta.Extratos).HasForeignKey(extrato => extrato.ContaId);
        }
    }
}
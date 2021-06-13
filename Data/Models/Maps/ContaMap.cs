using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models.Maps
{
    public class ContaMap : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            //Common Properties
            builder.HasKey(propertie => propertie.Id);
            builder.Property(propertie => propertie.DataCadastro).HasColumnType("date").IsRequired();
            builder.Property(propertie => propertie.DataAlteracao).HasColumnType("date").IsRequired();
            builder.Property(propertie => propertie.Status).HasColumnType("bit").IsRequired();

            //My Properties
            builder.Property(conta => conta.Numero).HasColumnType("varchar(12)").IsRequired();
            builder.Property(conta => conta.Saldo).HasColumnType("double").IsRequired();

            //Navigation Properties
            builder.HasOne(conta => conta.Usuario).WithOne(usuario => usuario.Conta).HasForeignKey<Conta>(conta => conta.IdUsuario);
            builder.HasMany(conta => conta.Extratos).WithOne(extrato => extrato.Conta);
        }
    }
}
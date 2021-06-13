using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models.Maps
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(usuario => usuario.Id);
            builder.Property(usuario => usuario.Nome).HasColumnType("varchar(500)").IsRequired();
            builder.Property(usuario => usuario.SobreNome).HasColumnType("varchar(500)").IsRequired();
            builder.Property(usuario => usuario.DataCadastro).HasColumnType("date").IsRequired();
            builder.Property(usuario => usuario.DataAlteracao).HasColumnType("date").IsRequired();
            builder.Property(usuario => usuario.Status).HasColumnType("bit").IsRequired();

            //Navigation Properties
            builder.HasOne(usuario => usuario.Conta).WithOne(conta => conta.Usuario);
        }
    }
}

using HubManPractices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Configuration
{
    class ClientConfiguration : EntityTypeConfiguration<Client>
    {
         public ClientConfiguration()
        {
            Property(c => c.ContactMail).HasMaxLength(60);
            Property(t => t.ContactMail).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute() { IsUnique = true }));
        }  
    }
}

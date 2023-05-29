using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESProj.Domain.Common;
public record DomainEvent
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public int Version { get; set; } = 1;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

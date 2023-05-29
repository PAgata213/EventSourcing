using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ESProj.Domain.Common;

namespace ESProj.Domain.VO;
public record ProductId(Guid Id) : ValueObject;

using BasePoint.Core.Domain.Entities;
using BasePoint.Core.Exceptions;
using BasePoint.Core.Tests.Common;

namespace BasePoint.Core.Tests.Domain.Entities
{
    public class SampleEntity : BaseEntity
    {
        public virtual string SampleName { get; set; }
        public decimal MonthlySalary { get; protected set; }

        public void SetMonthlySalary(decimal monthlySalary)
        {
            if (monthlySalary <= decimal.Zero)
                throw new ValidationException(CommonTestContants.EntitySalaryMustBeGreaterThanZero);

            MonthlySalary = monthlySalary;
        }
    }
}
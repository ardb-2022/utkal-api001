using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBWSFinanceApi.Models
{
    public class FortnightlyMasterList
    {
        public List<DemandFinancialYearPrincipal> Demand_Financial_Year_Principal { get; set; }
        public List<DemandFinancialYearInterest> Demand_Financial_Year_Interest { get; set; }
        public List<CollectionFortnightPrincipal> Collection_Fortnight_Principal { get; set; }
        public List<CollectionFortnightInterest> Collection_Fortnight_Interest { get; set; }
        public List<ProgressiveCollectionPrincipal> Progressive_Collection_Principal { get; set; }
        public List<ProgressiveCollectionInterest> Progressive_Collection_Interest { get; set; }
        public List<TargetLendingFinancialYear> Target_Lending_Financial_Year { get; set; }
        public List<LendingFortnight> Lending_Fortnight { get; set; }
        public List<ProgressiveLending> Progressive_Lending { get; set; }
        public List<CollectionRemittance> Collection_Remittance { get; set; }
        public List<Remittance> Remittance { get; set; }
        public List<FundPosition> Fund_Position { get; set; }

        public FortnightlyMasterList()
        {
            this.Demand_Financial_Year_Principal = new List<DemandFinancialYearPrincipal>();
            this.Demand_Financial_Year_Interest = new List<DemandFinancialYearInterest>();
            this.Collection_Fortnight_Principal = new List<CollectionFortnightPrincipal>();
            this.Collection_Fortnight_Interest = new List<CollectionFortnightInterest>();
            this.Progressive_Collection_Principal = new List<ProgressiveCollectionPrincipal>();
            this.Progressive_Collection_Interest = new List<ProgressiveCollectionInterest>();
            this.Target_Lending_Financial_Year = new List<TargetLendingFinancialYear>();
            this.Lending_Fortnight = new List<LendingFortnight>();
            this.Progressive_Lending = new List<ProgressiveLending>();
            this.Collection_Remittance = new List<CollectionRemittance>();
            this.Remittance = new List<Remittance>();
            this.Fund_Position = new List<FundPosition>();
        }
    }
}





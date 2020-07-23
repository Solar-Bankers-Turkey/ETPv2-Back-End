using System;

namespace CustomerService.Model {
    public class Detail {
        public Address address { get; set; }
        public string phoneNo { get; set; }
        public string identityNumber { get; set; }
        public DateTime birthDate { get; set; }
        public string region { get; set; }
        public string language { get; set; }
    }
}

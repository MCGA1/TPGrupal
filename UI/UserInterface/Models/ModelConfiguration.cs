using System.Collections.Generic;

namespace UserInterface.Models
{
  public class ModelConfiguration
  {
    public ServiceType Type { get; set; }
    public List<ModelConfigurationItem> Items { get; set; }
  }
}

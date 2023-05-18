namespace AtomWeb.Models
{
    public class ImageSliderVM
    {
        public List<KeyValuePair<string, string>> SliderData { get; set; } = new List<KeyValuePair<string, string>>();
        public string SliderTitle { get; set; } = "Slider";
    }
}

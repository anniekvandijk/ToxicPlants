// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

using System.Collections.Generic;

public class PlantNet
{
    public string language { get; set; }
    public string preferedReferential { get; set; }
    public string bestMatch { get; set; }
    public List<Result> results { get; set; }
    public string version { get; set; }
    public int remainingIdentificationRequests { get; set; }
}
public class Date
{
    public object timestamp { get; set; }
    public string @string { get; set; }
}

public class Family
{
    public string scientificNameWithoutAuthor { get; set; }
    public string scientificNameAuthorship { get; set; }
    public string scientificName { get; set; }
}

public class Gbif
{
    public string id { get; set; }
}

public class Genus
{
    public string scientificNameWithoutAuthor { get; set; }
    public string scientificNameAuthorship { get; set; }
    public string scientificName { get; set; }
}

public class Image
{
    public string organ { get; set; }
    public string author { get; set; }
    public string license { get; set; }
    public Date date { get; set; }
    public Url url { get; set; }
    public string citation { get; set; }
}

public class Result
{
    public double score { get; set; }
    public Species species { get; set; }
    public List<Image> images { get; set; } = new List<Image>();
    public Gbif gbif { get; set; }
}

public class Species
{
    public string scientificNameWithoutAuthor { get; set; }
    public string scientificNameAuthorship { get; set; }
    public Genus genus { get; set; }
    public Family family { get; set; }
    public List<string> commonNames { get; set; }
    public string scientificName { get; set; }
}

public class Url
{
    public string o { get; set; }
    public string m { get; set; }
    public string s { get; set; }
}


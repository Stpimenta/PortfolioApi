using Microsoft.EntityFrameworkCore;

namespace PortfolioApi.Domain.ValueObjects
{
    [Owned]
    public class GitConfig
    {
        public List<Git>? Gits { get; set; }
    }

    [Owned]
    public class Git
    {
        public string? Name { get; set; }
        public string? Link { get; set; }
    }

    [Owned]
    public class DownloadConfig
    {
        public List<Platform>? Plataforms { get; set; }
        public List<Step>? Steps { get; set; }
    }

    [Owned]
    public class Platform
    {
        public string? Name { get; set; }
        public List<Download>? Downloads { get; set; }
    }

    [Owned]
    public class Download
    {
        public string? Name { get; set; }
        public string? Link { get; set; }
    }

    [Owned]
    public class Step
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
    }

    [Owned]
    public class DocumentConfig
    {
        public List<Document>? Documents { get; set; }
    }

    [Owned]
    public class Document
    {
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public string? Link { get; set; }
    }
}
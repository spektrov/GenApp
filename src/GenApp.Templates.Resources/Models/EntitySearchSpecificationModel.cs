﻿using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class EntitySearchSpecificationModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.EntitySearchSpecification;

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }

    public string SpecificationName { get; set; }

    public string EntityName { get; set; }

    public string KeyType { get; set; }

    public string PropertyName { get; set; }
}
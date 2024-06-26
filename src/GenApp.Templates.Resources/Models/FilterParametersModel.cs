﻿using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class FilterParametersModel : BaseTemplateModel, ICsharpClassModel
{
    public override string TemplateName => TemplateNames.FilterParameters;

    public string Namespace { get; set; }

    public IEnumerable<string>? Usings { get; set; }
}

﻿using GenApp.Templates.Abstractions;

namespace GenApp.Templates.Resources.Models;
public class AppsettingsModel : BaseTemplateModel
{
    public override string TemplateName => TemplateNames.Appsettings;

    public string ConnectionString { get; set; }
}

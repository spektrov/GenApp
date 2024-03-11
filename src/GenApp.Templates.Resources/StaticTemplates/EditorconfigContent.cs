﻿namespace GenApp.Templates.Resources.StaticTemplates;
public static class EditorconfigContent
{
    public static string Value =>
		@"# NOTE: Requires **VS2019 16.3** or later

# Microsoft Managed Recommended Rules
# Description: These rules focus on the most critical problems in your code, including potential security holes, application crashes, and other important logic and design errors. It is recommended to include this rule set in any custom rule set you create for your projects.

# Code files
[*.{cs,vb}]


dotnet_diagnostic.CA1001.severity = warning

dotnet_diagnostic.CA1009.severity = warning

dotnet_diagnostic.CA1016.severity = warning

dotnet_diagnostic.CA1033.severity = warning

dotnet_diagnostic.CA1049.severity = warning

dotnet_diagnostic.CA1060.severity = warning

dotnet_diagnostic.CA1061.severity = warning

dotnet_diagnostic.CA1063.severity = warning

dotnet_diagnostic.CA1065.severity = warning

dotnet_diagnostic.CA1301.severity = warning

dotnet_diagnostic.CA1400.severity = warning

dotnet_diagnostic.CA1401.severity = warning

dotnet_diagnostic.CA1403.severity = warning

dotnet_diagnostic.CA1404.severity = warning

dotnet_diagnostic.CA1405.severity = warning

dotnet_diagnostic.CA1410.severity = warning

dotnet_diagnostic.CA1415.severity = warning

dotnet_diagnostic.CA1821.severity = warning

dotnet_diagnostic.CA1825.severity = none

dotnet_diagnostic.CA1900.severity = warning

dotnet_diagnostic.CA1901.severity = warning

dotnet_diagnostic.CA2002.severity = warning

dotnet_diagnostic.CA2100.severity = warning

dotnet_diagnostic.CA2101.severity = warning

dotnet_diagnostic.CA2108.severity = warning

dotnet_diagnostic.CA2111.severity = warning

dotnet_diagnostic.CA2112.severity = warning

dotnet_diagnostic.CA2114.severity = warning

dotnet_diagnostic.CA2116.severity = warning

dotnet_diagnostic.CA2117.severity = warning

dotnet_diagnostic.CA2122.severity = warning

dotnet_diagnostic.CA2123.severity = warning

dotnet_diagnostic.CA2124.severity = warning

dotnet_diagnostic.CA2126.severity = warning

dotnet_diagnostic.CA2131.severity = warning

dotnet_diagnostic.CA2132.severity = warning

dotnet_diagnostic.CA2133.severity = warning

dotnet_diagnostic.CA2134.severity = warning

dotnet_diagnostic.CA2137.severity = warning

dotnet_diagnostic.CA2138.severity = warning

dotnet_diagnostic.CA2140.severity = warning

dotnet_diagnostic.CA2141.severity = warning

dotnet_diagnostic.CA2146.severity = warning

dotnet_diagnostic.CA2147.severity = warning

dotnet_diagnostic.CA2149.severity = warning

dotnet_diagnostic.CA2200.severity = warning

dotnet_diagnostic.CA2202.severity = warning

dotnet_diagnostic.CA2207.severity = warning

dotnet_diagnostic.CA2212.severity = warning

dotnet_diagnostic.CA2213.severity = warning

dotnet_diagnostic.CA2214.severity = warning

dotnet_diagnostic.CA2216.severity = warning

dotnet_diagnostic.CA2220.severity = warning

dotnet_diagnostic.CA2229.severity = warning

dotnet_diagnostic.CA2231.severity = warning

dotnet_diagnostic.CA2232.severity = warning

dotnet_diagnostic.CA2235.severity = warning

dotnet_diagnostic.CA2236.severity = warning

dotnet_diagnostic.CA2237.severity = warning

dotnet_diagnostic.CA2238.severity = warning

dotnet_diagnostic.CA2240.severity = warning

dotnet_diagnostic.CA2241.severity = warning

dotnet_diagnostic.CA2242.severity = warning

dotnet_diagnostic.SA0001.severity = none

dotnet_diagnostic.SA0002.severity = error

dotnet_diagnostic.SA1000.severity = error

dotnet_diagnostic.SA1001.severity = error

dotnet_diagnostic.SA1002.severity = error

dotnet_diagnostic.SA1003.severity = error

dotnet_diagnostic.SA1004.severity = error

dotnet_diagnostic.SA1005.severity = error

dotnet_diagnostic.SA1006.severity = error

dotnet_diagnostic.SA1007.severity = error

dotnet_diagnostic.SA1008.severity = error

dotnet_diagnostic.SA1009.severity = error

dotnet_diagnostic.SA1010.severity = error

dotnet_diagnostic.SA1011.severity = none

dotnet_diagnostic.SA1012.severity = error

dotnet_diagnostic.SA1013.severity = error

dotnet_diagnostic.SA1014.severity = error

dotnet_diagnostic.SA1015.severity = error

dotnet_diagnostic.SA1016.severity = error

dotnet_diagnostic.SA1017.severity = error

dotnet_diagnostic.SA1018.severity = error

dotnet_diagnostic.SA1019.severity = error

dotnet_diagnostic.SA1020.severity = error

dotnet_diagnostic.SA1021.severity = error

dotnet_diagnostic.SA1022.severity = error

dotnet_diagnostic.SA1023.severity = error

dotnet_diagnostic.SA1024.severity = error

dotnet_diagnostic.SA1025.severity = error

dotnet_diagnostic.SA1026.severity = error

dotnet_diagnostic.SA1027.severity = error

dotnet_diagnostic.SA1028.severity = error

dotnet_diagnostic.SA1100.severity = error

dotnet_diagnostic.SA1101.severity = none

dotnet_diagnostic.SA1102.severity = error

dotnet_diagnostic.SA1103.severity = error

dotnet_diagnostic.SA1104.severity = error

dotnet_diagnostic.SA1105.severity = error

dotnet_diagnostic.SA1106.severity = error

dotnet_diagnostic.SA1107.severity = error

dotnet_diagnostic.SA1108.severity = error

dotnet_diagnostic.SA1110.severity = error

dotnet_diagnostic.SA1111.severity = error

dotnet_diagnostic.SA1112.severity = error

dotnet_diagnostic.SA1113.severity = error

dotnet_diagnostic.SA1114.severity = error

dotnet_diagnostic.SA1115.severity = error

dotnet_diagnostic.SA1116.severity = error

dotnet_diagnostic.SA1117.severity = none

dotnet_diagnostic.SA1118.severity = none

dotnet_diagnostic.SA1119.severity = error

dotnet_diagnostic.SA1120.severity = error

dotnet_diagnostic.SA1121.severity = error

dotnet_diagnostic.SA1122.severity = none

dotnet_diagnostic.SA1123.severity = none

dotnet_diagnostic.SA1124.severity = none

dotnet_diagnostic.SA1125.severity = error

dotnet_diagnostic.SA1127.severity = error

dotnet_diagnostic.SA1128.severity = none

dotnet_diagnostic.SA1129.severity = none

dotnet_diagnostic.SA1130.severity = error

dotnet_diagnostic.SA1131.severity = error

dotnet_diagnostic.SA1132.severity = error

dotnet_diagnostic.SA1133.severity = error

dotnet_diagnostic.SA1134.severity = error

dotnet_diagnostic.SA1136.severity = error

dotnet_diagnostic.SA1137.severity = error

dotnet_diagnostic.SA1139.severity = none

dotnet_diagnostic.SA1200.severity = none

dotnet_diagnostic.SA1201.severity = none

dotnet_diagnostic.SA1202.severity = error

dotnet_diagnostic.SA1203.severity = error

dotnet_diagnostic.SA1204.severity = error

dotnet_diagnostic.SA1205.severity = error

dotnet_diagnostic.SA1206.severity = error

dotnet_diagnostic.SA1207.severity = error

dotnet_diagnostic.SA1208.severity = none

dotnet_diagnostic.SA1209.severity = error

dotnet_diagnostic.SA1210.severity = none

dotnet_diagnostic.SA1211.severity = none

dotnet_diagnostic.SA1212.severity = error

dotnet_diagnostic.SA1213.severity = error

dotnet_diagnostic.SA1214.severity = error

dotnet_diagnostic.SA1216.severity = error

dotnet_diagnostic.SA1217.severity = none

dotnet_diagnostic.SA1300.severity = none

dotnet_diagnostic.SA1302.severity = error

dotnet_diagnostic.SA1303.severity = error

dotnet_diagnostic.SA1304.severity = error

dotnet_diagnostic.SA1306.severity = error

dotnet_diagnostic.SA1307.severity = error

dotnet_diagnostic.SA1308.severity = error

dotnet_diagnostic.SA1309.severity = none

dotnet_diagnostic.SA1310.severity = none

dotnet_diagnostic.SA1311.severity = none

dotnet_diagnostic.SA1312.severity = error

dotnet_diagnostic.SA1313.severity = error

dotnet_diagnostic.SA1314.severity = error

dotnet_diagnostic.SA1400.severity = error

dotnet_diagnostic.SA1401.severity = error

dotnet_diagnostic.SA1402.severity = none

dotnet_diagnostic.SA1403.severity = error

dotnet_diagnostic.SA1404.severity = error

dotnet_diagnostic.SA1405.severity = error

dotnet_diagnostic.SA1406.severity = error

dotnet_diagnostic.SA1407.severity = none

dotnet_diagnostic.SA1408.severity = none

dotnet_diagnostic.SA1410.severity = none

dotnet_diagnostic.SA1411.severity = error

dotnet_diagnostic.SA1412.severity = none

dotnet_diagnostic.SA1413.severity = none

dotnet_diagnostic.SA1500.severity = error

dotnet_diagnostic.SA1501.severity = error

dotnet_diagnostic.SA1502.severity = none

dotnet_diagnostic.SA1503.severity = none

dotnet_diagnostic.SA1504.severity = error

dotnet_diagnostic.SA1505.severity = error

dotnet_diagnostic.SA1506.severity = none

dotnet_diagnostic.SA1507.severity = error

dotnet_diagnostic.SA1508.severity = error

dotnet_diagnostic.SA1509.severity = error

dotnet_diagnostic.SA1510.severity = error

dotnet_diagnostic.SA1511.severity = error

dotnet_diagnostic.SA1512.severity = none

dotnet_diagnostic.SA1513.severity = error

dotnet_diagnostic.SA1514.severity = none

dotnet_diagnostic.SA1515.severity = error

dotnet_diagnostic.SA1516.severity = error

dotnet_diagnostic.SA1517.severity = error

dotnet_diagnostic.SA1518.severity = error

dotnet_diagnostic.SA1519.severity = error

dotnet_diagnostic.SA1520.severity = error

dotnet_diagnostic.SA1600.severity = none

dotnet_diagnostic.SA1601.severity = none

dotnet_diagnostic.SA1602.severity = none

dotnet_diagnostic.SA1604.severity = none

dotnet_diagnostic.SA1605.severity = none

dotnet_diagnostic.SA1606.severity = none

dotnet_diagnostic.SA1607.severity = none

dotnet_diagnostic.SA1608.severity = none

dotnet_diagnostic.SA1610.severity = none

dotnet_diagnostic.SA1611.severity = none

dotnet_diagnostic.SA1612.severity = none

dotnet_diagnostic.SA1613.severity = none

dotnet_diagnostic.SA1614.severity = none

dotnet_diagnostic.SA1615.severity = none

dotnet_diagnostic.SA1616.severity = none

dotnet_diagnostic.SA1617.severity = none

dotnet_diagnostic.SA1618.severity = none

dotnet_diagnostic.SA1619.severity = none

dotnet_diagnostic.SA1620.severity = none

dotnet_diagnostic.SA1621.severity = none

dotnet_diagnostic.SA1622.severity = none

dotnet_diagnostic.SA1623.severity = none

dotnet_diagnostic.SA1624.severity = none

dotnet_diagnostic.SA1625.severity = none

dotnet_diagnostic.SA1626.severity = none

dotnet_diagnostic.SA1627.severity = none

dotnet_diagnostic.SA1633.severity = none

dotnet_diagnostic.SA1634.severity = none

dotnet_diagnostic.SA1635.severity = none

dotnet_diagnostic.SA1636.severity = none

dotnet_diagnostic.SA1637.severity = none

dotnet_diagnostic.SA1638.severity = none

dotnet_diagnostic.SA1640.severity = none

dotnet_diagnostic.SA1641.severity = none

dotnet_diagnostic.SA1642.severity = none

dotnet_diagnostic.SA1643.severity = none

dotnet_diagnostic.SA1648.severity = none

dotnet_diagnostic.SA1649.severity = error

dotnet_diagnostic.SA1651.severity = none

dotnet_diagnostic.SA1652.severity = none

dotnet_diagnostic.S2360.severity = none

# IDE0058: Expression value is never used
csharp_style_unused_value_expression_statement_preference = discard_variable:silent

[*.{cs,vb}]
#### Naming styles ####

# Naming rules

dotnet_naming_rule.interface_should_be_begins_with_i.severity = suggestion
dotnet_naming_rule.interface_should_be_begins_with_i.symbols = interface
dotnet_naming_rule.interface_should_be_begins_with_i.style = begins_with_i

dotnet_naming_rule.types_should_be_pascal_case.severity = suggestion
dotnet_naming_rule.types_should_be_pascal_case.symbols = types
dotnet_naming_rule.types_should_be_pascal_case.style = pascal_case

dotnet_naming_rule.non_field_members_should_be_pascal_case.severity = suggestion
dotnet_naming_rule.non_field_members_should_be_pascal_case.symbols = non_field_members
dotnet_naming_rule.non_field_members_should_be_pascal_case.style = pascal_case

# Symbol specifications

dotnet_naming_symbols.interface.applicable_kinds = interface
dotnet_naming_symbols.interface.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected
dotnet_naming_symbols.interface.required_modifiers = 

dotnet_naming_symbols.types.applicable_kinds = class, struct, interface, enum
dotnet_naming_symbols.types.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected
dotnet_naming_symbols.types.required_modifiers = 

dotnet_naming_symbols.non_field_members.applicable_kinds = property, event, method
dotnet_naming_symbols.non_field_members.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected
dotnet_naming_symbols.non_field_members.required_modifiers = 

# Naming styles

dotnet_naming_style.begins_with_i.required_prefix = I
dotnet_naming_style.begins_with_i.required_suffix = 
dotnet_naming_style.begins_with_i.word_separator = 
dotnet_naming_style.begins_with_i.capitalization = pascal_case

dotnet_naming_style.pascal_case.required_prefix = 
dotnet_naming_style.pascal_case.required_suffix = 
dotnet_naming_style.pascal_case.word_separator = 
dotnet_naming_style.pascal_case.capitalization = pascal_case

dotnet_naming_style.pascal_case.required_prefix = 
dotnet_naming_style.pascal_case.required_suffix = 
dotnet_naming_style.pascal_case.word_separator = 
dotnet_naming_style.pascal_case.capitalization = pascal_case
dotnet_style_coalesce_expression = true:suggestion
dotnet_style_null_propagation = true:suggestion
dotnet_style_prefer_is_null_check_over_reference_equality_method = true:suggestion
dotnet_style_prefer_auto_properties = true:silent
dotnet_style_object_initializer = true:suggestion
dotnet_style_collection_initializer = true:suggestion
dotnet_style_prefer_simplified_boolean_expressions = true:suggestion
dotnet_style_prefer_conditional_expression_over_assignment = true:silent
dotnet_style_prefer_conditional_expression_over_return = true:silent
dotnet_style_explicit_tuple_names = true:suggestion
dotnet_style_prefer_inferred_tuple_names = true:suggestion
dotnet_style_operator_placement_when_wrapping = beginning_of_line
tab_width = 4
indent_size = 4
end_of_line = crlf
dotnet_style_prefer_inferred_anonymous_type_member_names = true:suggestion
dotnet_style_prefer_compound_assignment = true:suggestion
dotnet_style_prefer_simplified_interpolation = true:suggestion
dotnet_style_namespace_match_folder = true:suggestion
dotnet_style_readonly_field = true:suggestion
dotnet_style_predefined_type_for_locals_parameters_members = true:silent
dotnet_style_predefined_type_for_member_access = true:silent
dotnet_style_require_accessibility_modifiers = for_non_interface_members:silent
dotnet_style_allow_multiple_blank_lines_experimental = true:silent
dotnet_style_allow_statement_immediately_after_block_experimental = true:silent
dotnet_code_quality_unused_parameters = all:suggestion
dotnet_style_parentheses_in_arithmetic_binary_operators = always_for_clarity:silent
dotnet_style_parentheses_in_other_binary_operators = always_for_clarity:silent
dotnet_style_parentheses_in_relational_binary_operators = always_for_clarity:silent
dotnet_style_parentheses_in_other_operators = never_if_unnecessary:silent
dotnet_style_qualification_for_field = false:silent
dotnet_style_qualification_for_property = false:silent
dotnet_style_qualification_for_method = false:silent
dotnet_style_qualification_for_event = false:silent
dotnet_style_prefer_collection_expression = true:suggestion

[*.cs]
csharp_using_directive_placement = outside_namespace:silent
csharp_prefer_simple_using_statement = true:suggestion
csharp_prefer_braces = true:silent
csharp_style_namespace_declarations = file_scoped:silent
csharp_style_prefer_method_group_conversion = true:silent
csharp_style_prefer_top_level_statements = true:silent
csharp_style_expression_bodied_methods = false:silent
csharp_style_expression_bodied_constructors = false:silent
csharp_style_expression_bodied_operators = false:silent
csharp_style_expression_bodied_properties = true:silent
csharp_style_expression_bodied_indexers = true:silent
csharp_style_expression_bodied_accessors = true:silent
csharp_style_expression_bodied_lambdas = true:silent
csharp_style_expression_bodied_local_functions = false:silent
csharp_indent_labels = no_change
csharp_style_throw_expression = true:suggestion
csharp_style_prefer_null_check_over_type_check = true:suggestion
csharp_prefer_simple_default_expression = true:suggestion
csharp_style_prefer_local_over_anonymous_function = true:suggestion
csharp_style_prefer_index_operator = true:suggestion
csharp_style_prefer_range_operator = true:suggestion
csharp_style_implicit_object_creation_when_type_is_apparent = true:suggestion
csharp_style_prefer_tuple_swap = true:suggestion
csharp_style_prefer_utf8_string_literals = true:suggestion
csharp_style_inlined_variable_declaration = true:suggestion
csharp_style_deconstructed_variable_declaration = true:suggestion
csharp_style_unused_value_assignment_preference = discard_variable:suggestion
csharp_style_unused_value_expression_statement_preference = discard_variable:silent
csharp_prefer_static_local_function = true:suggestion
csharp_style_prefer_readonly_struct = true:suggestion
csharp_style_allow_embedded_statements_on_same_line_experimental = true:silent
csharp_style_allow_blank_lines_between_consecutive_braces_experimental = true:silent
csharp_style_allow_blank_line_after_colon_in_constructor_initializer_experimental = true:silent
csharp_style_conditional_delegate_call = true:suggestion
csharp_style_prefer_switch_expression = true:suggestion
csharp_style_prefer_pattern_matching = true:silent
csharp_style_pattern_matching_over_is_with_cast_check = true:suggestion
csharp_style_pattern_matching_over_as_with_null_check = true:suggestion
csharp_style_prefer_not_pattern = true:suggestion
csharp_style_prefer_extended_property_pattern = true:suggestion
csharp_style_var_for_built_in_types = false:silent
csharp_style_var_when_type_is_apparent = false:silent
csharp_style_var_elsewhere = false:silent
dotnet_diagnostic.SA1009.severity = silent
dotnet_diagnostic.SA1003.severity = silent
csharp_space_around_binary_operators = before_and_after
csharp_style_allow_blank_line_after_token_in_conditional_expression_experimental = true:silent
csharp_style_allow_blank_line_after_token_in_arrow_expression_clause_experimental = true:silent
dotnet_diagnostic.SA1000.severity = silent
dotnet_diagnostic.SA1401.severity = suggestion
csharp_style_prefer_primary_constructors = true:suggestion
csharp_style_prefer_readonly_struct_member = true:suggestion";
}

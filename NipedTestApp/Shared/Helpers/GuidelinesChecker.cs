using Shared.DataModels;

namespace Shared.Helpers;

public static class GuidelinesChecker
{
    public static MeasurementStatus ClassifyMeasurement(string value, Guideline guideline)
    {
        var splitValue = value.Split(' ');
        var optimalStringMatches = 0;
        var needsAttentionStringMatches = 0;
        var seriousIssueStringMatches = 0;
        foreach (var str in splitValue)
        {
            if (guideline.Optimal.Contains(str)) optimalStringMatches++;
            if (guideline.NeedsAttention.Contains(str)) needsAttentionStringMatches++;
            if (guideline.SeriousIssue.Contains(str)) seriousIssueStringMatches++;
        }

        var measurementStatus = MeasurementStatus.Unknown;
        if (optimalStringMatches >= needsAttentionStringMatches && optimalStringMatches >= seriousIssueStringMatches)
        {
            measurementStatus = MeasurementStatus.Optimal;
        }
        else if (needsAttentionStringMatches >= optimalStringMatches &&
                 needsAttentionStringMatches >= seriousIssueStringMatches)
        {
            measurementStatus = MeasurementStatus.NeedsAttention;
        }
        else if (seriousIssueStringMatches >= optimalStringMatches &&
                 seriousIssueStringMatches >= needsAttentionStringMatches)
        {
            measurementStatus = MeasurementStatus.SeriousIssue;
        }
        
        return measurementStatus;
    }
    
    public static MeasurementStatus ClassifyMeasurement(double value, Guideline guideline)
    {
        if (MatchesRange(value, guideline.Optimal))
            return MeasurementStatus.Optimal;

        if (MatchesRange(value, guideline.NeedsAttention))
            return MeasurementStatus.NeedsAttention;

        if (MatchesRange(value, guideline.SeriousIssue))
            return MeasurementStatus.SeriousIssue;

        return MeasurementStatus.Unknown;
    }
    
    private static bool MatchesRange(double value, string rule)
    {
        rule = rule.Trim();

        // Range like "100-129"
        if (rule.Contains("-") && double.TryParse(rule.Split('-')[0], out var min) &&
            double.TryParse(rule.Split('-')[1], out var max))
        {
            return value >= min && value <= max;
        }

        // >= value
        if (rule.StartsWith(">=") && double.TryParse(rule.Substring(2), out var minInclusive))
        {
            return value >= minInclusive;
        }

        // <= value
        if (rule.StartsWith("<=") && double.TryParse(rule.Substring(2), out var maxInclusive))
        {
            return value <= maxInclusive;
        }

        // > value
        if (rule.StartsWith(">") && double.TryParse(rule.Substring(1), out var minExclusive))
        {
            return value > minExclusive;
        }

        // < value
        if (rule.StartsWith("<") && double.TryParse(rule.Substring(1), out var maxExclusive))
        {
            return value < maxExclusive;
        }

        // Exact match (rare)
        if (double.TryParse(rule, out var exact))
        {
            return Math.Abs(value - exact) < 0.0001;
        }

        return false; // Non-numeric / descriptive strings
    }
}
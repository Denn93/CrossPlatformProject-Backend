using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessObjects;

/// <summary>
/// Summary description for SourceMapping
/// </summary>
public class SourceMapper
{
    public MatchyMatcher.MatchyBackend.Source MapToService(Source source)
    {
        MatchyMatcher.MatchyBackend.Source result = new MatchyMatcher.MatchyBackend.Source();

        if (source != null)
        {
            return new MatchyMatcher.MatchyBackend.Source() { SourceId = source.SourceId, Description = source.Description };
        }
        else
            return result;
    }

    public Source MapFromService(MatchyMatcher.MatchyBackend.Source source)
    {
        return new Source() { SourceId = source.SourceId, Description = source.Description };
    }
}
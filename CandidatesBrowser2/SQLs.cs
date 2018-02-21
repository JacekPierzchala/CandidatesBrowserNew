using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser2
{
    class SQLs
    {
        public static string Candidates =
            @"SELECT [ID],[FIRST_NAME],[LAST_NAME] ,[1ST_@]
            ,[2ND_@],[1ST_TEL],[2ND_TEL]
        FROM [CANDIDATES]";
    }
}

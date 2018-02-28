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
            @"
            IF OBJECT_ID('tempdb.dbo.#CAND', 'U') IS NOT NULL DROP TABLE #CAND; 
            IF OBJECT_ID('tempdb.dbo.#PROJECTS', 'U') IS NOT NULL DROP TABLE #PROJECTS;
            IF OBJECT_ID('tempdb.dbo.#CV_REC', 'U') IS NOT NULL DROP TABLE #CV_REC;

            SELECT [ID],[FIRST_NAME],[LAST_NAME] ,[1ST_@]
                 ,[2ND_@],[1ST_TEL],[2ND_TEL]
                INTO #CAND
            FROM [CANDIDATES]

        SELECT DISTINCT SC.CANDIDATE_ID, 
        COUNT (CONFIG_PROJECT_ID) OVER (PARTITION BY SC.CANDIDATE_ID) ATTENDED_PROJECTS
        INTO #PROJECTS
        FROM #CAND INNER JOIN [STATUS_CHANGES] SC ON #CAND.ID=SC.CANDIDATE_ID

        SELECT DISTINCT SC.CANDIDATE_ID
        INTO #CV_REC
        FROM #CAND INNER JOIN [STATUS_CHANGES] SC ON #CAND.ID=SC.CANDIDATE_ID
        WHERE CV_RECEIVED=1


        SELECT [ID],[FIRST_NAME],[LAST_NAME] ,[1ST_@]
     ,[2ND_@],[1ST_TEL],[2ND_TEL],ATTENDED_PROJECTS,
      CAST (
             (CASE WHEN #CV_REC.CANDIDATE_ID IS NOT NULL THEN 1
             ELSE 0 END) AS BIT) CV_RECEIVED
         FROM #CAND INNER JOIN #PROJECTS ON #CAND.ID=#PROJECTS.CANDIDATE_ID
			        LEFT JOIN #CV_REC ON #CAND.ID=#CV_REC.CANDIDATE_ID";
    }
}

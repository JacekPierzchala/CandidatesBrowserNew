using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser2
{
    class SQLs
    {

        #region CandidatesInitialView
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

        #endregion

        #region Statuses
        public static string Statuses = @"SELECT  [ID],[DESCRIPTION],[DEFINITION],[DELETED]
                                        FROM CONFIG_STATUS_LIB ORDER BY DESCRIPTION";

        #endregion

        #region Area
        public static string Area = @"SELECT [ID],[AREA_NAME]
                                    FROM [CONFIG_AREA] ORDER BY AREA_NAME";

        #endregion

        #region ProjectsLib
        public static string Projects = @"SELECT [ID],[PROJECT_NAME]
                                          FROM [CONFIG_PROJECT_LIB] ORDER BY PROJECT_NAME";
        #endregion

        #region ProjectGroup
        public static string ProjectGroup = @"SELECT     dbo.CONFIG_PROJECT.ID, dbo.CONFIG_PROJECT.CONFIG_PROJECT_LIB, dbo.CONFIG_PROJECT.CONFIG_AREA_ID, 
                       dbo.CONFIG_PROJECT.CONFIG_GROUP_ID
                       FROM         dbo.CONFIG_PROJECT INNER JOIN
                      dbo.CONFIG_PROJECT_LIB ON dbo.CONFIG_PROJECT.CONFIG_PROJECT_LIB = dbo.CONFIG_PROJECT_LIB.ID INNER JOIN
                      dbo.CONFIG_GROUP ON dbo.CONFIG_PROJECT.CONFIG_GROUP_ID = dbo.CONFIG_GROUP.ID INNER JOIN
                      dbo.CONFIG_AREA ON dbo.CONFIG_PROJECT.CONFIG_AREA_ID = dbo.CONFIG_AREA.ID";
        #endregion

        #region Group
        public static string Groups = @"SELECT  [ID],[NAME]
                                        FROM [CONFIG_GROUP] ORDER BY NAME";
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace OperationData.Common
{
  public static  class LogHelper
  {
      private static readonly ILog loginfo = LogManager.GetLogger("loginfo");
      private static readonly ILog logerror = LogManager.GetLogger("logerror");

      public static void WriteLog(string info)
      {
          if (loginfo.IsInfoEnabled)
          {
              loginfo.Info(info);
          }
      }

      public static void WriteLog(string info, Exception ex)
      {
          if (logerror.IsErrorEnabled)
          {
              logerror.Error(info, ex);
          }
      }
  }
}

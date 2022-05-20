using System.Collections.Generic;

namespace AuthServer.Constants;

public static class Permissions {
    public static List<string> GeneratePermissionsForModule(string module) {
        return new List<string>() {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
        };
    }

    public static class Depts {
        public const string View = "Permissions.Depts.View";
        public const string Create = "Permissions.Depts.Create";
        public const string Edit = "Permissions.Depts.Edit";
        public const string Delete = "Permissions.Depts.Delete";
    }
}


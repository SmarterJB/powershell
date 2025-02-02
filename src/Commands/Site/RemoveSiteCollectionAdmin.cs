﻿using System.Management.Automation;
using Microsoft.SharePoint.Client;

using System.Collections.Generic;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Remove, "PnPSiteCollectionAdmin")]
    public class RemoveSiteCollectionAdmin : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public List<UserPipeBind> Owners;

        protected override void ExecuteCmdlet()
        {
            foreach (var owner in Owners)
            {
                User user = owner.GetUser(ClientContext, true);

                if (user != null)
                {
                    user.IsSiteAdmin = false;
                    user.Update();

                    try
                    {
                        ClientContext.ExecuteQueryRetry();
                    }
                    catch (ServerException e)
                    {
                        WriteWarning($"Exception occurred while trying to remove the user: \"{e.Message}\". User will be skipped.");
                    }
                }
                else
                {
                    WriteWarning($"Unable to remove user as it wasn't found. User will be skipped.");
                }
            }
        }
    }
}
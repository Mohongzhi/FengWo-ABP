using System;
using System.Collections.Generic;
using System.Data.Common;
using Abp.Data;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using FengWo.EntityFrameworkCore;
using FengWo.EntityFrameworkCore.Seed;
using FengWo.MultiTenancy;

namespace FengWo.Migrator
{
    public class MultiTenantMigrateExecuter : ITransientDependency
    {
        private readonly Log _log;
        private readonly AbpZeroDbMigrator _migrator;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;

        public MultiTenantMigrateExecuter(
            AbpZeroDbMigrator migrator,
            IRepository<Tenant> tenantRepository,
            Log log,
            IDbPerTenantConnectionStringResolver connectionStringResolver)
        {
            _log = log;

            _migrator = migrator;
            _tenantRepository = tenantRepository;
            _connectionStringResolver = connectionStringResolver;
        }

        public bool Run(bool skipConnVerification)
        {
            var hostConnStr = CensorConnectionString(_connectionStringResolver.GetNameOrConnectionString(new ConnectionStringResolveArgs(MultiTenancySides.Host)));
            if (hostConnStr.IsNullOrWhiteSpace())
            {
                _log.Write("配置文件应包含名为\"Default\"的连接串");
                return false;
            }

            _log.Write("数据库连接串: " + ConnectionStringHelper.GetConnectionString(hostConnStr));
            _log.Write("Database connection string: " + ConnectionStringHelper.GetConnectionString(hostConnStr));
            if (!skipConnVerification)
            {
                _log.Write("是否迁移数据库...? (Y/N): ");
                _log.Write("Whether to migrate databases? (Y/N): ");
                var command = Console.ReadLine();
                if (!command.IsIn("Y", "y"))
                {
                    _log.Write("已取消迁移.");
                    return false;
                }
            }

            _log.Write("数据库迁移开始...");
            _log.Write("Database migration starts...");

            try
            {
                _migrator.CreateOrMigrateForHost(SeedHelper.SeedHostDb);
            }
            catch (Exception ex)
            {
                _log.Write("迁移数据库期间发生错误:");
                _log.Write("Errors occurred during migrating databases:");
                _log.Write(ex.ToString());
                _log.Write("Migration has been cancelled评.");
                _log.Write("迁移已取消.");
                return false;
            }
            _log.Write("数据库迁移已完成.");
            _log.Write("Database migration completed.");
            _log.Write("--------------------------------------------------------");

            var migratedDatabases = new HashSet<string>();
            var tenants = _tenantRepository.GetAllList(t => t.ConnectionString != null && t.ConnectionString != "");
            for (var i = 0; i < tenants.Count; i++)
            {
                var tenant = tenants[i];
                _log.Write(string.Format("Tenant database migration started... ({0} / {1})", (i + 1), tenants.Count));
                _log.Write("Name              : " + tenant.Name);
                _log.Write("TenancyName       : " + tenant.TenancyName);
                _log.Write("Tenant Id         : " + tenant.Id);
                _log.Write("Connection string : " + SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString));

                if (!migratedDatabases.Contains(tenant.ConnectionString))
                {
                    try
                    {
                        _migrator.CreateOrMigrateForTenant(tenant);
                    }
                    catch (Exception ex)
                    {
                        _log.Write("An error occured during migration of tenant database:");
                        _log.Write(ex.ToString());
                        _log.Write("Skipped this tenant and will continue for others...");
                    }

                    migratedDatabases.Add(tenant.ConnectionString);
                }
                else
                {
                    _log.Write("This database has already migrated before (you have more than one tenant in same database). Skipping it....");
                }

                _log.Write(string.Format("Tenant database migration completed. ({0} / {1})", (i + 1), tenants.Count));
                _log.Write("--------------------------------------------------------");
            }

            _log.Write("已迁移所有数据库.");
            _log.Write("All databases migrated.");

            return true;
        }

        private static string CensorConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            var keysToMask = new[] { "password", "pwd", "user id", "uid" };

            foreach (var key in keysToMask)
            {
                if (builder.ContainsKey(key))
                {
                    builder[key] = "*****";
                }
            }

            return builder.ToString();
        }
    }
}

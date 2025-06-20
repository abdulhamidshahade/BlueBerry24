import { RoleStats } from '@/types/roleManagement';

interface RoleManagementStatsProps {
  stats: RoleStats;
}

export default function RoleManagementStats({ stats }: RoleManagementStatsProps) {
  const statsCards = [
    {
      name: 'Total Roles',
      value: stats.totalRoles,
      icon: (
        <svg width="24" height="24" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" d="M9 12.75L11.25 15 15 9.75m-3-7.036A11.959 11.959 0 013.598 6 11.99 11.99 0 003 9.749c0 5.592 3.824 10.29 9 11.623 5.176-1.332 9-6.03 9-11.623 0-1.31-.21-2.571-.598-3.751h-.152c-3.196 0-6.1-1.248-8.25-3.285z" />
        </svg>
      ),
      bgColor: 'bg-primary',
      textColor: 'text-primary',
      bgColorLight: 'bg-primary bg-opacity-10',
      subtitle: stats.additionalStats ? `${stats.additionalStats.systemRoles} system, ${stats.additionalStats.customRoles} custom` : undefined
    },
    {
      name: 'Total Users',
      value: stats.totalUsers,
      icon: (
        <svg width="24" height="24" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" d="M15 19.128a9.38 9.38 0 002.625.372 9.337 9.337 0 004.121-.952 4.125 4.125 0 00-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 718.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0111.964-3.07M12 6.375a3.375 3.375 0 11-6.75 0 3.375 3.375 0 016.75 0zm8.25 2.25a2.625 2.625 0 11-5.25 0 2.625 2.625 0 015.25 0z" />
        </svg>
      ),
      bgColor: 'bg-success',
      textColor: 'text-success',
      bgColorLight: 'bg-success bg-opacity-10',
    },
    {
      name: 'Users with Roles',
      value: stats.usersWithRoles,
      icon: (
        <svg width="24" height="24" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" d="M18 18.72a9.094 9.094 0 003.741-.479 3 3 0 00-4.682-2.72m.94 3.198l.001.031c0 .225-.012.447-.037.666A11.944 11.944 0 0112 21c-2.17 0-4.207-.576-5.963-1.584A6.062 6.062 0 016 18.719m12 0a5.971 5.971 0 00-.941-3.197m0 0A5.995 5.995 0 0012 12.75a5.995 5.995 0 00-5.058 2.772m0 0a3 3 0 00-4.681 2.72 8.986 8.986 0 003.74.477m.94-3.197a5.971 5.971 0 00-.94 3.197M15 6.75a3 3 0 11-6 0 3 3 0 016 0zm6 3a2.25 2.25 0 11-4.5 0 2.25 2.25 0 014.5 0zm-13.5 0a2.25 2.25 0 11-4.5 0 2.25 2.25 0 014.5 0z" />
        </svg>
      ),
      bgColor: 'bg-info',
      textColor: 'text-info',
      bgColorLight: 'bg-info bg-opacity-10',
    },
    {
      name: 'Users without Roles',
      value: stats.usersWithoutRoles,
      icon: (
        <svg width="24" height="24" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" d="M22 10.5h-6m-2.25-4.125a3.375 3.375 0 11-6.75 0 3.375 3.375 0 016.75 0zM4 19.235v-.11a6.375 6.375 0 0112.674-1.334c.343.684.526 1.445.526 2.227 0 .312-.026.614-.073.904M4 19.235a5.994 5.994 0 010-.114z" />
        </svg>
      ),
      bgColor: 'bg-warning',
      textColor: 'text-warning',
      bgColorLight: 'bg-warning bg-opacity-10',
    },
  ];

  const usersWithRolesPercentage = stats.totalUsers > 0 
    ? Math.round((stats.usersWithRoles / stats.totalUsers) * 100) 
    : 0;

  const avgRolesPerUser = stats.totalUsers > 0 
    ? (stats.usersWithRoles / stats.totalUsers).toFixed(1) 
    : '0';

  return (
    <div className="mb-5">
      <div className="row g-4 mb-4">
        {statsCards.map((stat) => (
          <div key={stat.name} className="col-12 col-md-6 col-lg-3">
            <div className="card h-100 shadow-sm">
              <div className="card-body p-4">
                <div className="d-flex align-items-center">
                  <div className="flex-shrink-0">
                    <div className={`${stat.bgColorLight} ${stat.textColor} p-3 rounded`}>
                      {stat.icon}
                    </div>
                  </div>
                  <div className="ms-3 flex-grow-1">
                    <div className="text-muted small fw-medium text-truncate">
                      {stat.name}
                    </div>
                    <div className="h5 fw-medium text-dark mb-0">
                      {stat.value.toLocaleString()}
                    </div>
                    {stat.subtitle && (
                      <div className="text-muted small">
                        {stat.subtitle}
                      </div>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>

      <div className="card shadow-sm">
        <div className="card-body p-4">
          <h3 className="h5 fw-medium text-dark mb-4">
            Role Assignment Overview
          </h3>
          <div className="row g-4">
            <div className="col-12 col-md-6">
              <div className="d-flex justify-content-between align-items-center mb-2">
                <span className="small fw-medium text-body">Role Assignment Coverage</span>
                <span className="small text-muted">{usersWithRolesPercentage}%</span>
              </div>
              <div className="progress mb-2" style={{height: '8px'}}>
                <div 
                  className="progress-bar bg-primary" 
                  role="progressbar" 
                  style={{ width: `${usersWithRolesPercentage}%` }}
                  aria-valuenow={usersWithRolesPercentage}
                  aria-valuemin={0}
                  aria-valuemax={100}
                ></div>
              </div>
              <p className="text-muted mb-0" style={{fontSize: '0.75rem'}}>
                {stats.usersWithRoles} of {stats.totalUsers} users have assigned roles
              </p>
            </div>

            <div className="col-12 col-md-6">
              <div className="d-flex flex-column gap-3">
                <div className="d-flex justify-content-between align-items-center">
                  <span className="small text-muted">Role Coverage Rate:</span>
                  <span className={`small fw-medium ${
                    usersWithRolesPercentage >= 80 ? 'text-success' : 
                    usersWithRolesPercentage >= 60 ? 'text-warning' : 'text-danger'
                  }`}>
                    {usersWithRolesPercentage}%
                  </span>
                </div>
                <div className="d-flex justify-content-between align-items-center">
                  <span className="small text-muted">Avg Roles per User:</span>
                  <span className="small fw-medium text-dark">
                    {avgRolesPerUser}
                  </span>
                </div>
                <div className="d-flex justify-content-between align-items-center">
                  <span className="small text-muted">Users Needing Roles:</span>
                  <span className={`small fw-medium ${
                    stats.usersWithoutRoles > 0 ? 'text-warning' : 'text-success'
                  }`}>
                    {stats.usersWithoutRoles}
                  </span>
                </div>
              </div>
            </div>
          </div>

          {stats.additionalStats && stats.additionalStats.usersPerRole.length > 0 && (
            <div className="mt-4">
              <h6 className="fw-medium text-dark mb-3">Role Distribution</h6>
              <div className="row g-3">
                {stats.additionalStats.usersPerRole
                  .filter(role => role.userCount > 0)
                  .sort((a, b) => b.userCount - a.userCount)
                  .slice(0, 6)
                  .map((role, index) => {
                    const percentage = stats.totalUsers > 0 
                      ? Math.round((role.userCount / stats.totalUsers) * 100) 
                      : 0;
                    
                    const colors = [
                      'bg-primary', 'bg-success', 'bg-info', 
                      'bg-warning', 'bg-danger', 'bg-secondary'
                    ];
                    
                    return (
                      <div key={role.roleName} className="col-12 col-md-6 col-lg-4">
                        <div className="d-flex justify-content-between align-items-center mb-1">
                          <span className="small fw-medium text-truncate me-2">{role.roleName}</span>
                          <span className="small text-muted">{role.userCount}</span>
                        </div>
                        <div className="progress" style={{ height: '6px' }}>
                          <div 
                            className={`progress-bar ${colors[index % colors.length]}`}
                            style={{ width: `${percentage}%` }}
                          />
                        </div>
                        <div className="small text-muted mt-1">
                          {percentage}% of users
                        </div>
                      </div>
                    );
                  })
                }
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
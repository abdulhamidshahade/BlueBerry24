import Link from 'next/link';

interface RoleManagementTabsProps {
  activeTab: string;
  searchParams: {
    tab?: string;
    search?: string;
    page?: string;
    expanded?: string;
    edit?: string;
    expand?: string;
    user?: string;
    error?: string;
    success?: string;
    role?: string;
  };
  rolesCount?: number;
  usersCount?: number;
}

export default function RoleManagementTabs({ 
  activeTab, 
  searchParams,
  rolesCount,
  usersCount
}: RoleManagementTabsProps) {
  const createTabUrl = (tab: string) => {
    const params = new URLSearchParams();
    params.set('tab', tab);
    
    if (searchParams.search && (
      (tab === 'roles' && activeTab === 'roles') ||
      (tab === 'users' && activeTab === 'users')
    )) {
      params.set('search', searchParams.search);
    }
    
    if (tab === 'users' && activeTab === 'users' && searchParams.page) {
      params.set('page', searchParams.page);
    }
    
    if (tab === 'users' && searchParams.role) {
      params.set('role', searchParams.role);
    }
    
    return `/admin/role-management?${params.toString()}`;
  };

  const tabItems = [
    {
      id: 'overview',
      label: 'Overview',
      icon: (
        <svg className="me-2" width="16" height="16" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" d="M3.75 3v11.25A2.25 2.25 0 006 16.5h2.25M3.75 3h-1.5m1.5 0h16.5m0 0h1.5m-1.5 0v11.25A2.25 2.25 0 0018 16.5h-2.25m-7.5 0h7.5m-7.5 0l-1 3m8.5-3l1 3m0 0l-1-3m1 3l-1-3m-16.5 0h2.25" />
        </svg>
      ),
      count: null
    },
    {
      id: 'roles',
      label: 'Roles',
      icon: (
        <svg className="me-2" width="16" height="16" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" d="M9 12.75L11.25 15 15 9.75m-3-7.036A11.959 11.959 0 013.598 6 11.99 11.99 0 013 9.749c0 5.592 3.824 10.29 9 11.623 5.176-1.332 9-6.03 9-11.623 0-1.31-.21-2.571-.598-3.751h-.152c-3.196 0-6.1-1.248-8.25-3.285z" />
        </svg>
      ),
      count: rolesCount
    },
    {
      id: 'users',
      label: 'Users',
      icon: (
        <svg className="me-2" width="16" height="16" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" d="M15 19.128a9.38 9.38 0 002.625.372 9.337 9.337 0 004.121-.952 4.125 4.125 0 00-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 018.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0111.964-3.07M12 6.375a3.375 3.375 0 11-6.75 0 3.375 3.375 0 016.75 0zm8.25 2.25a2.625 2.625 0 11-5.25 0 2.625 2.625 0 015.25 0z" />
        </svg>
      ),
      count: usersCount
    }
  ];

  return (
    <div className="mb-4">
      <div className="card shadow-sm">
        <div className="card-body p-0">
          <ul className="nav nav-tabs border-0" role="tablist">
            {tabItems.map((tab) => (
              <li key={tab.id} className="nav-item" role="presentation">
                <a
                  href={createTabUrl(tab.id)}
                  className={`nav-link d-flex align-items-center px-4 py-3 ${
                    activeTab === tab.id 
                      ? 'active border-bottom border-primary border-2' 
                      : 'text-muted'
                  }`}
                  role="tab"
                  aria-selected={activeTab === tab.id}
                >
                  {tab.icon}
                  <span className="fw-medium">{tab.label}</span>
                  {tab.count !== null && tab.count !== undefined && (
                    <span className={`badge ms-2 ${
                      activeTab === tab.id 
                        ? 'bg-primary' 
                        : 'bg-secondary'
                    }`}>
                      {tab.count}
                    </span>
                  )}
                </a>
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
}
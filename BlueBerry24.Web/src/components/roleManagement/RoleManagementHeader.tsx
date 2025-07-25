import Link from 'next/link';
import { User } from '../../types/user';

interface RoleManagementHeaderProps {
  currentUser: User;
}

export default function RoleManagementHeader({ currentUser }: RoleManagementHeaderProps) {
  return (
    <div className="bg-white shadow-sm border-bottom">
      <div className="container-fluid">
        <div className="d-flex justify-content-between align-items-center py-4">
          <div>
            <nav aria-label="breadcrumb">
              <ol className="breadcrumb mb-2">
                <li className="breadcrumb-item">
                  <Link href="/admin" className="text-muted text-decoration-none">
                    <svg className="me-1" width="16" height="16" viewBox="0 0 20 20" fill="currentColor">
                      <path d="M10.707 2.293a1 1 0 00-1.414 0l-7 7a1 1 0 001.414 1.414L4 10.414V17a1 1 0 001 1h2a1 1 0 001-1v-2a1 1 0 011-1h2a1 1 0 011 1v2a1 1 0 001 1h2a1 1 0 001-1v-6.586l.293.293a1 1 0 001.414-1.414l-7-7z" />
                    </svg>
                    <span className="visually-hidden">Admin Dashboard</span>
                  </Link>
                </li>
                <li className="breadcrumb-item">
                  <Link href="/admin" className="text-muted text-decoration-none">
                    Admin
                  </Link>
                </li>
                <li className="breadcrumb-item active" aria-current="page">
                  Role Management
                </li>
              </ol>
            </nav>
            <div>
              <h1 className="h2 fw-bold text-dark mb-1">
                Role Management
              </h1>
              <p className="text-muted small mb-0">
                Manage user roles and permissions across the BlueBerry24 system
              </p>
            </div>
          </div>

          <div className="d-flex align-items-center">
            <div className="text-end me-3">
              <p className="fw-medium mb-0 small">
                {currentUser.firstName} {currentUser.lastName}
              </p>
              <p className="text-muted mb-0" style={{fontSize: '0.75rem'}}>
                {currentUser.roles.join(', ')}
              </p>
            </div>
            <div className="bg-primary rounded-circle d-flex align-items-center justify-content-center" 
                 style={{width: '32px', height: '32px'}}>
              <span className="text-white fw-medium small">
                {currentUser.firstName?.[0]}{currentUser.lastName?.[0]}
              </span>
            </div>
          </div>
        </div>
      </div>

      <div className="bg-warning bg-opacity-10 border-top border-warning">
        <div className="container-fluid">
          <div className="py-3">
            <div className="d-flex align-items-center">
              <div className="flex-shrink-0 me-3">
                <svg className="text-warning" width="20" height="20" viewBox="0 0 20 20" fill="currentColor">
                  <path fillRule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clipRule="evenodd" />
                </svg>
              </div>
              <div>
                <p className="text-warning-emphasis small mb-0">
                  <strong>Super Admin Access:</strong> You have full control over user roles and permissions. 
                  Please use these capabilities responsibly.
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
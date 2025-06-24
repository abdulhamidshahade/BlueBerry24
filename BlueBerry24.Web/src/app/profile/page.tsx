import { getCurrentUser } from "@/lib/actions/auth-actions";
import { redirect } from "next/navigation";

export default async function ProfilePage() {
  const user = await getCurrentUser();

  if (!user) {
    redirect("/auth/login?redirectTo=/profile");
  }

  return (
    <div className="container py-5">
      <div className="row">
        <div className="col-md-8 mx-auto">
          <div className="card shadow">
            <div className="card-header bg-primary text-white">
              <h2 className="card-title mb-0">
                <i className="bi bi-person-circle me-2"></i>
                My Profile
              </h2>
            </div>
            <div className="card-body p-4">
              <div className="row">
                <div className="col-md-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">
                      <i className="bi bi-person me-1"></i>
                      Username
                    </label>
                    <p className="form-control-plaintext">{user.userName}</p>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">
                      <i className="bi bi-envelope me-1"></i>
                      Email Address
                    </label>
                    <p className="form-control-plaintext">{user.email}</p>
                  </div>
                </div>
              </div>

              <div className="row">
                <div className="col-md-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">
                      <i className="bi bi-person-badge me-1"></i>
                      First Name
                    </label>
                    <p className="form-control-plaintext">
                      {user.firstName || "Not provided"}
                    </p>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="mb-3">
                    <label className="form-label fw-bold">
                      <i className="bi bi-person-badge me-1"></i>
                      Last Name
                    </label>
                    <p className="form-control-plaintext">
                      {user.lastName || "Not provided"}
                    </p>
                  </div>
                </div>
              </div>

              <div className="mb-3">
                <label className="form-label fw-bold">
                  <i className="bi bi-shield-check me-1"></i>
                  Account Roles
                </label>
                <div className="d-flex gap-2 flex-wrap">
                  {user.roles.map((role) => (
                    <span key={role} className="badge bg-secondary">
                      {role}
                    </span>
                  ))}
                </div>
              </div>

              <hr />

              <div className="d-flex gap-2">
                <a href="" className="btn btn-primary">
                  <i className="bi bi-pencil me-1"></i>
                  Edit Profile
                </a>
                <a
                  href=""
                  className="btn btn-outline-secondary"
                >
                  <i className="bi bi-key me-1"></i>
                  Change Password
                </a>
              </div>
            </div>
          </div>

          <div className="row mt-4">
            <div className="col-md-4">
              <div className="card">
                <div className="card-body text-center">
                  <i className="bi bi-box-seam display-4 text-primary"></i>
                  <h5 className="card-title mt-2">My Orders</h5>
                  <p className="card-text text-muted">
                    View and track your orders from here
                  </p>
                  <a href="/orders" className="btn btn-outline-primary">
                    View Orders
                  </a>
                </div>
              </div>
            </div>
            <div className="col-md-4">
              <div className="card">
                <div className="card-body text-center">
                  <i className="bi bi-ticket-detailed display-4 text-success"></i>
                  <h5 className="card-title mt-2">My Coupons</h5>
                  <p className="card-text text-muted">
                    View and use your discount coupons
                  </p>
                  <a
                    href="/profile/coupons"
                    className="btn btn-outline-success"
                  >
                    View Coupons
                  </a>
                </div>
              </div>
            </div>
            <div className="col-md-4">
              <div className="card">
                <div className="card-body text-center">
                  <i className="bi bi-heart display-4 text-danger"></i>
                  <h5 className="card-title mt-2">Wishlist</h5>
                  <p className="card-text text-muted">
                    Manage your favorite items from here
                  </p>
                  <a
                    href="/profile/wishlist"
                    className="btn btn-outline-danger"
                  >
                    View Wishlist
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: "My Profile - BlueBerry24",
  description: "Manage your BlueBerry24 account profile",
};

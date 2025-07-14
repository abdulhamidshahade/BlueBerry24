import { exportOrders } from '../../lib/actions/order-actions';

export function ExportOrdersModal() {
  return (
    <div className="modal fade" id="exportModal" tabIndex={-1}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Export Orders</h5>
            <button type="button" className="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <form action={exportOrders}>
            <div className="modal-body">
              <div className="mb-3">
                <label htmlFor="exportStatus" className="form-label">Filter by Status</label>
                <select className="form-select" id="exportStatus" name="status">
                  <option value="all">All Orders</option>
                  <option value="0">Pending</option>
                  <option value="1">Processing</option>
                  <option value="2">Shipped</option>
                  <option value="3">Delivered</option>
                  <option value="4">Completed</option>
                  <option value="5">Cancelled</option>
                  <option value="6">Refunded</option>
                </select>
              </div>
              <div className="mb-3">
                <label htmlFor="exportFormat" className="form-label">Export Format</label>
                <select className="form-select" id="exportFormat" name="format">
                  <option value="csv">CSV</option>
                  <option value="excel">Excel</option>
                  <option value="pdf">PDF</option>
                </select>
              </div>
              <div className="alert alert-info">
                <i className="bi bi-info-circle me-2"></i>
                This will prepare the export file with the selected filters.
              </div>
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">
                Close
              </button>
              <button type="submit" className="btn btn-primary">
                <i className="bi bi-download me-2"></i>Export Orders
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
} 
import { useEffect, useState } from "react";
import { useAuth } from "../AuthContext";
import { Link, useNavigate } from "react-router-dom";

type Report = {
  id: number;
  itemId: number;
  reason: string;
  createdAt: string;
  status: string;
};

type ReportedItem = {
  itemId: number;
  name: string;
  description?: string;
  ownerId?: string;
  reportCount: number;
  reports: Report[];
};

type ReportedComment = {
  commentId: number;
  itemId: number;
  itemName: string;
  commentText: string;
  reportCount: number;
  reports: Report[];
};

const API = "http://localhost:5145/api";

export default function Moderation() {
  const { user, loading } = useAuth();
  const navigate = useNavigate();
  const [items, setItems] = useState<ReportedItem[]>([]);
  const [comments, setComments] = useState<ReportedComment[]>([]);
  const [error, setError] = useState<string | null>(null);

  const loadReports = async () => {
    const [itemsResponse, commentsResponse] = await Promise.all([
      fetch(`${API}/reports`, { credentials: "include" }),
      fetch(`${API}/reports/comments`, { credentials: "include" }),
    ]);
    if ([itemsResponse.status, commentsResponse.status].some((status) => status === 401 || status === 403)) {
      setError("Neturite moderatoriaus teisių.");
      return;
    }
    if (!itemsResponse.ok || !commentsResponse.ok) {
      setError("Nepavyko gauti pranešimų.");
      return;
    }
    setItems(await itemsResponse.json());
    setComments(await commentsResponse.json());
  };

  useEffect(() => {
    if (!loading && (!user || !user.roles.includes("Moderator"))) {
      queueMicrotask(() => navigate("/"));
      return;
    }
    if (user?.roles.includes("Moderator")) {
      void loadReports();
    }
  }, [loading, user, navigate]);

  const dismiss = async (reportId: number) => {
    const response = await fetch(`${API}/reports/${reportId}/dismiss`, {
      method: "POST",
      credentials: "include",
    });
    if (response.ok) void loadReports();
  };

  const deleteItem = async (itemId: number) => {
    if (!window.confirm("Ar tikrai norite ištrinti šį skelbimą?")) return;
    const response = await fetch(`${API}/items/${itemId}`, {
      method: "DELETE",
      credentials: "include",
    });
    if (response.ok) void loadReports();
  };

  const deleteComment = async (itemId: number, commentId: number) => {
    if (!window.confirm("Ar tikrai norite ištrinti šį komentarą?")) return;
    const response = await fetch(`${API}/items/${itemId}/comments/${commentId}`, {
      method: "DELETE",
      credentials: "include",
    });
    if (response.ok) void loadReports();
  };

  if (loading || !user?.roles.includes("Moderator")) return null;

  return (
    <div className="container">
      <h2 className="mb-4">Pranešti skelbimai</h2>
      {error && <div className="alert alert-warning">{error}</div>}
      {items.length === 0 && comments.length === 0 && !error && <p className="text-muted">Aktyvių pranešimų nėra.</p>}
      <div className="d-flex flex-column gap-3">
        {items.map((item) => (
          <div className="card" key={item.itemId}>
            <div className="card-body">
              <h5 className="card-title">
                <Link to={`/item/${item.itemId}`}>{item.name}</Link>
                <span className="badge text-bg-warning ms-2">{item.reportCount} praneš.</span>
              </h5>
              <p className="card-text">{item.description}</p>
              {item.reports.map((report) => (
                <div className="border-top pt-2 mt-2" key={report.id}>
                  <div>{report.reason}</div>
                  <small className="text-muted">{new Date(report.createdAt).toLocaleString()}</small>
                  <div className="mt-2 d-flex gap-2">
                    <button className="btn btn-outline-secondary btn-sm" onClick={() => void dismiss(report.id)}>Atmesti pranešimą</button>
                    <button className="btn btn-danger btn-sm" onClick={() => void deleteItem(item.itemId)}>Ištrinti skelbimą</button>
                  </div>
                  {comments.length > 0 && (
                    <>
                      <h2 className="mb-4 mt-5">Pranešti komentarai</h2>
                      <div className="d-flex flex-column gap-3">
                        {comments.map((comment) => (
                          <div className="card" key={comment.commentId}>
                            <div className="card-body">
                              <h5 className="card-title">
                                <Link to={`/item/${comment.itemId}`}>{comment.itemName}</Link>
                                <span className="badge text-bg-warning ms-2">{comment.reportCount} praneš.</span>
                              </h5>
                              <p className="card-text">{comment.commentText}</p>
                              {comment.reports.map((report) => (
                                <div className="border-top pt-2 mt-2" key={report.id}>
                                  <div>{report.reason}</div>
                                  <small className="text-muted">{new Date(report.createdAt).toLocaleString()}</small>
                                  <div className="mt-2 d-flex gap-2">
                                    <button className="btn btn-outline-secondary btn-sm" onClick={() => void dismiss(report.id)}>Atmesti pranešimą</button>
                                    <button className="btn btn-danger btn-sm" onClick={() => void deleteComment(comment.itemId, comment.commentId)}>Ištrinti komentarą</button>
                                  </div>
                                </div>
                              ))}
                            </div>
                          </div>
                        ))}
                      </div>
                    </>
                  )}
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

import React, { useState, useEffect } from 'react';
import { useAuth } from '../AuthContext';

const API_BASE = "http://localhost:5145/api/items";

interface Comment {
    id: number;
    itemId: number;
    text: string;
    createdAt: string;
    isOwner: boolean;
}

interface Props {
    itemId: number;
}

export const ItemInteractions: React.FC<Props> = ({ itemId }) => {
    const { user } = useAuth();
    const [likes, setLikes] = useState<number>(0);
    const [isLiked, setIsLiked] = useState<boolean>(false);
    const [comments, setComments] = useState<Comment[]>([]);
    const [newComment, setNewComment] = useState<string>('');
    const [error, setError] = useState<string | null>(null);
    const [reportReason, setReportReason] = useState('');
    const [showReportForm, setShowReportForm] = useState(false);
    const [commentReportId, setCommentReportId] = useState<number | null>(null);
    const [commentReportReason, setCommentReportReason] = useState('');

    useEffect(() => {
        fetchLikes();
        fetchComments();
    }, [itemId, user]);

    const fetchLikes = async () => {
        try {
            const res = await fetch(`${API_BASE}/${itemId}/likes`, {
                credentials: 'include',
            });
            if (res.ok) {
                const data = await res.json();
                setLikes(data.totalLikes);
                if (typeof data.isLiked === 'boolean') {
                    setIsLiked(data.isLiked);
                }
            }
        } catch (err) {
            console.error('Klaida gaunant patiktukus:', err);
        }
    };

    const fetchComments = async () => {
        try {
            const res = await fetch(`${API_BASE}/${itemId}/comments`, {
                credentials: 'include',
            });
            if (res.ok) {
                const data = await res.json();
                setComments(data);
            }
        } catch (err) {
            console.error('Klaida gaunant komentarus:', err);
        }
    };

    const handleLike = async () => {
        setError(null);
        try {
            const res = await fetch(`${API_BASE}/${itemId}/like`, {
                method: 'POST',
                credentials: 'include',
            });
            if (res.ok) {
                const data = await res.json();
                setLikes(data.totalLikes);
                setIsLiked(data.liked);
            } else if (res.status === 401) {
                setError('Norėdami pamėgti, turite prisijungti.');
            }
        } catch (err) {
            console.error('Klaida dedant patiktuką:', err);
        }
    };

    const handleAddComment = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!newComment.trim()) return;

        setError(null);
        try {
            const res = await fetch(`${API_BASE}/${itemId}/comments`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                credentials: 'include',
                body: JSON.stringify({ text: newComment }),
            });

            if (res.ok) {
                setNewComment('');
                fetchComments();
            } else if (res.status === 401) {
                setError('Norėdami komentuoti, turite prisijungti.');
            }
        } catch (err) {
            console.error('Klaida pridedant komentarą:', err);
        }
    };

    const handleDeleteComment = async (commentId: number) => {
        if (!window.confirm("Ar tikrai norite ištrinti šį komentarą?")) return;

        try {
            const res = await fetch(`${API_BASE}/${itemId}/comments/${commentId}`, {
                method: 'DELETE',
                credentials: 'include',
            });

            if (res.ok) {
                setComments((prev) => prev.filter((c) => c.id !== commentId));
            } else if (res.status === 401 || res.status === 404) {
                alert("Nepavyko ištrinti: galite trinti tik savo komentarus.");
            }
        } catch (err) {
            console.error("Klaida trinant komentarą:", err);
        }
    };

    const handleReport = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!reportReason.trim()) return;

        const res = await fetch(`${API_BASE}/${itemId}/reports`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            body: JSON.stringify({ reason: reportReason }),
        });

        if (res.ok) {
            setReportReason('');
            setShowReportForm(false);
            setError('Pranešimas išsiųstas moderatoriui.');
        } else if (res.status === 409) {
            setError('Šį skelbimą jau pranešėte.');
        } else if (res.status === 401) {
            setError('Norėdami pranešti, turite prisijungti.');
        }
    };

    const handleReportComment = async (e: React.FormEvent, commentId: number) => {
        e.preventDefault();
        if (!commentReportReason.trim()) return;

        const res = await fetch(`http://localhost:5145/api/comments/${commentId}/reports`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            body: JSON.stringify({ reason: commentReportReason }),
        });

        if (res.ok) {
            setCommentReportId(null);
            setCommentReportReason('');
            setError('Komentaro pranešimas išsiųstas moderatoriui.');
        } else if (res.status === 409) {
            setError('Šį komentarą jau pranešėte.');
        } else if (res.status === 401) {
            setError('Norėdami pranešti, turite prisijungti.');
        }
    };

    return (
        <div className="mt-3 pt-3 border-top">
            <div className="d-flex align-items-center gap-2 mb-2">
                <button
                    type="button"
                    onClick={handleLike}
                    className={`btn btn-sm ${isLiked ? 'btn-danger' : 'btn-outline-danger'}`}
                >
                    {isLiked ? '❤️ Pamėgta' : '🤍 Patinka'} ({likes})
                </button>
            </div>

            {error && <div className="alert alert-warning py-1 px-2 my-2 small">{error}</div>}

            {user && (
                showReportForm ? (
                    <form onSubmit={handleReport} className="d-flex gap-2 mb-3">
                        <input
                            className="form-control form-control-sm"
                            value={reportReason}
                            onChange={(e) => setReportReason(e.target.value)}
                            placeholder="Pranešimo priežastis"
                        />
                        <button className="btn btn-warning btn-sm" type="submit">Pranešti</button>
                        <button className="btn btn-outline-secondary btn-sm" type="button" onClick={() => setShowReportForm(false)}>Atšaukti</button>
                    </form>
                ) : (
                    <button className="btn btn-outline-warning btn-sm mb-3" type="button" onClick={() => setShowReportForm(true)}>
                        Pranešti apie skelbimą
                    </button>
                )
            )}

            <div className="mb-3">
                <h6 className="mb-2">Komentarai ({comments.length}):</h6>
                {comments.length === 0 ? (
                    <p className="text-muted small">Komentarų dar nėra.</p>
                ) : (
                    <div className="d-flex flex-column gap-2" style={{ maxHeight: '180px', overflowY: 'auto' }}>
                        {comments.map((c) => (
                            <React.Fragment key={c.id}>
                            <div className="bg-light p-2 rounded small d-flex justify-content-between align-items-start">
                                <div>
                                    <div>{c.text}</div>
                                    <div className="text-muted" style={{ fontSize: '11px' }}>
                                        {new Date(c.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                                    </div>
                                </div>
                                {user && (
                                    <div className="d-flex gap-1">
                                        {(c.isOwner || user.roles.includes("Moderator")) && (
                                            <button
                                                type="button"
                                                onClick={() => handleDeleteComment(c.id)}
                                                className="btn btn-outline-secondary btn-sm border-0 py-0 px-1 text-danger"
                                                title="Ištrinti komentarą"
                                                style={{ fontSize: '12px' }}
                                            >
                                                ✕
                                            </button>
                                        )}
                                        <button
                                            type="button"
                                            onClick={() => setCommentReportId(commentReportId === c.id ? null : c.id)}
                                            className="btn btn-outline-warning btn-sm border-0 py-0 px-1"
                                            title="Pranešti apie komentarą"
                                            style={{ fontSize: '12px' }}
                                        >
                                            !
                                        </button>
                                    </div>
                                )}
                            </div>
                            {user && commentReportId === c.id && (
                                <form onSubmit={(e) => void handleReportComment(e, c.id)} className="d-flex gap-2">
                                    <input
                                        className="form-control form-control-sm"
                                        value={commentReportReason}
                                        onChange={(e) => setCommentReportReason(e.target.value)}
                                        placeholder="Pranešimo priežastis"
                                    />
                                    <button className="btn btn-warning btn-sm" type="submit">Pranešti</button>
                                </form>
                            )}
                            </React.Fragment>
                        ))}
                    </div>
                )}
            </div>

            <form onSubmit={handleAddComment} className="d-flex gap-2">
                <input
                    type="text"
                    className="form-control form-control-sm"
                    placeholder={user ? "Rašyti komentarą..." : "Prisijunkite, norėdami komentuoti"}
                    disabled={!user}
                    value={newComment}
                    onChange={(e) => setNewComment(e.target.value)}
                />
                <button type="submit" className="btn btn-primary btn-sm" disabled={!user}>
                    Siųsti
                </button>
            </form>
        </div>
    );
};
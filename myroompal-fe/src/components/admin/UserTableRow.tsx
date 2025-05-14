import { AdminUserSearchUser } from "../../models/AdminPanel/AdminUserSearchUser";
import EnumUserStatus from "../../models/EnumUserStatus";

interface UserTableRowProps {
    user: AdminUserSearchUser;
    onStatusChange: (user: AdminUserSearchUser, newStatus: EnumUserStatus) => void;
    onDeleteUser: (user: AdminUserSearchUser) => void;
}

function UserTableRow({ user, onStatusChange, onDeleteUser }: UserTableRowProps) {
    return (
        <tr>
            <td title={user.id.toString()}>{user.id.toString().substring(0, 8)}...</td>
            <td>{user.name}</td>
            <td>{user.email}</td>
            <td>{user.phoneNumber}</td>
            <td>{user.status}</td>
            <td>
                <div className="btn-group position-relative">
                    <button
                        type="button"
                        className="btn btn-warning btn-sm dropdown-toggle"
                        data-bs-toggle="dropdown"
                        aria-expanded="false"
                    >
                        Edit
                    </button>
                    <ul className="dropdown-menu dropdown-menu-end position-absolute">
                        <li>
                            <button
                                className="dropdown-item"
                                role="button"
                                onClick={() => onStatusChange(user, EnumUserStatus.BANNED)}
                                disabled={user.status === EnumUserStatus.BANNED}
                            >
                                Ban
                            </button>
                        </li>
                        <li>
                            <button
                                className="dropdown-item"
                                role="button"
                                onClick={() => onStatusChange(user, EnumUserStatus.UNBANNED)}
                                disabled={user.status === EnumUserStatus.UNBANNED}
                            >
                                Unban
                            </button>
                        </li>
                        <li>
                            <button
                                className="dropdown-item text-danger"
                                role="button"
                                onClick={() => onDeleteUser(user)}
                                disabled={true}
                            >
                                Delete
                            </button>
                        </li>
                    </ul>
                </div>
            </td>
        </tr>
    );
}

export default UserTableRow;

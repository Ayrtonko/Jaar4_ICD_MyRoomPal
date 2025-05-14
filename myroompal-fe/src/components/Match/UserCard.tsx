import {Button} from "react-bootstrap";
import React, {useState} from "react";
import axios from "axios";
import {config} from "../../config";

type TUserCardProps = {
    accountId: string
    name: string
    email: string
    matchScore: number
    matchBtn?: boolean
    chatBtn?: boolean
    deleteBtn?: boolean
    deleteToggle?: boolean
    setDeleteToggle?: React.Dispatch<React.SetStateAction<boolean>>

}

const UserCard = ({
                      accountId,
                      name,
                      email,
                      matchScore,
                      matchBtn,
                      chatBtn,
                      deleteBtn,
                      deleteToggle,
                      setDeleteToggle
                  }: TUserCardProps) => {

    const [btnIsDisabled, setBtnIsDisabled] = useState<boolean>(false);

    const matchBtnOnClick = (matchedUserId: string) => {
        const url = `${config.apiBaseUrl}/matching/create-like?likeeUserId=${matchedUserId.toUpperCase()}`
        axios.post(url)
            .catch(e => {
                console.log(e)
            })
        setBtnIsDisabled(true);
    }

    const handleDeleteMatch = async () => {
        const url = `${config.apiBaseUrl}/matching/delete-match`
        await axios.delete(url, {
            data: {
                LikeeUserId: accountId,
            },
        }).then(() => {
            if (setDeleteToggle) {
                setDeleteToggle(!deleteToggle)
            }
        })
            .catch(e => {
                console.log(e)
            })
    }

    return (
        <div className="col-12 d-flex  flex-wrap gap-2">
            <div className="col d-flex col-md-6 col-12">
                <div className="user-icon align-content-center">
                    <i className="bi-person-fill text-white px-2" style={{fontSize: "24px"}}></i>
                </div>
                <div className="user-card col  d-flex justify-content-between">
                    <p className="mb-0">
                        {name}, {email}
                    </p>
                    <p className="mb-0">
                        {matchScore != null ? "Score:" : ""} {matchScore}
                    </p>
                </div>

            </div>

            <div className="col d-flex align-self-start justify-content-end justify-content-md-start gap-2">
                {
                    chatBtn ?
                        <Button variant="primary" className="d-flex align-items-center"
                                onClick={() => matchBtnOnClick(accountId)}
                                disabled={true}
                        >
                            Chat
                            <i className="bi-chat-dots ms-2" style={{fontSize: "24px"}}></i>
                        </Button>
                        : null
                }
                {
                    matchBtn ?
                        <Button variant="secondary" className="d-flex align-items-center"
                                onClick={() => matchBtnOnClick(accountId)}
                                disabled={btnIsDisabled}
                        >
                            Match
                            <i className="bi-check2-circle ms-2" style={{fontSize: "24px"}}></i>
                        </Button>
                        : null
                }
                {deleteBtn ?
                    <Button variant="danger"
                            onClick={handleDeleteMatch}>
                        <i className="bi-x-circle mx-1"
                           style={{fontSize: "24px"}}>
                        </i>
                    </Button>
                    : null}
            </div>
        </div>
    );
};

export default UserCard;

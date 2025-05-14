import React, {useEffect, useState} from "react";
import {Button} from "react-bootstrap";
import MatchList from "../MatchList";
import MatchLinks from "../MatchLinks";
import {config} from "../../../config";
import axios from "axios";
import TProfileResponse from "../../../models/Match/ProfileResponseType";


type TMatchSectionProps = {
    locationValue: string,
    selectedPreferences: string[]
}
const MatchSection = ({locationValue, selectedPreferences}: TMatchSectionProps) => {
    const [hasMounted, setHasMounted] = useState<boolean>(false)
    const [unlikedProfiles, setUnlikedProfiles] = useState<TProfileResponse[]>([])
    const [matchedProfiles, setMatchedProfiles] = useState<TProfileResponse[]>([])

    const [showMatches, setShowMatches] = useState<boolean>(false)
    const [matchesToggle, setMatchesToggle] = useState<boolean>(false)
    const [deleteToggle, setDeleteToggle] = useState<boolean>(false)
    const [preferencesAreSetToggle, setPreferencesAreSetToggle] = useState<boolean>(false)
    const [selectedMatchNav, setSelectedMatchNav] = useState<number>(0)

    useEffect(() => {
        if (!hasMounted) {
            setHasMounted(true);
        }
    }, []);

    useEffect(() => {
        if (hasMounted) {
            const requestDataPreferences = {
                preferences: selectedPreferences,
            };
            const requestDataSearchLocation = {
                searchLocation: locationValue
            }
            const urlCreatePreferences = `${config.apiBaseUrl}/matching/create-preferences-user`
            const urlUpdateSearchLocation = `${config.apiBaseUrl}/matching/update-profile-searchlocation`

            Promise.all([
                axios.post(urlCreatePreferences, requestDataPreferences)
                    .catch(e => {
                        console.log(e);
                    }),
                axios.put(urlUpdateSearchLocation, requestDataSearchLocation)
                    .catch(e => {
                        console.log(e);
                    })
            ]).then(() => {
                setPreferencesAreSetToggle(!preferencesAreSetToggle)
            });
        }
    }, [showMatches, matchesToggle]);

    useEffect(() => {
        if (hasMounted) {
                axios.get(`${config.apiBaseUrl}/matching/unliked-profiles`)
                    .then(res => {
                        setUnlikedProfiles(res.data)
                    })
                    .catch((e) => {
                        console.log(e)
                    })
        }
    }, [preferencesAreSetToggle]);

    useEffect(() => {
        if (hasMounted) {
                axios.get(`${config.apiBaseUrl}/matching/user-match-profiles`)
                    .then(res => {
                        setMatchedProfiles(res.data)
                    })
                    .catch((e) => {
                        console.log(e)
                    })
        }
    }, [preferencesAreSetToggle, deleteToggle]);

    return (
        <div className="row g-0">
            <div className="px-5">
                <h2 className=" display-6 mt-5">Let's get you matched!</h2>
                <p>Make sure you have filled in the location and all your preferences.</p>
                <div className="d-flex col">
                    {!showMatches &&
                        <Button variant={"secondary"} cy-data="find-matches" onClick={() => setShowMatches(true)}>Find matches</Button>}
                </div>
                {
                    showMatches ?
                        <>
                            <MatchLinks selectedMatchNav={selectedMatchNav} setSelectedMatchNav={setSelectedMatchNav}/>
                            <div className={`${selectedMatchNav === 0 ? 'd-block' : 'd-none'}`}><MatchList
                                profiles={unlikedProfiles} matchBtn={true} chatBtn={true} /></div>
                            <div className={`${selectedMatchNav === 1 ? 'd-block' : 'd-none'}`}><MatchList
                                profiles={matchedProfiles} chatBtn={true} deleteBtn={true} deleteToggle={deleteToggle} setDeleteToggle={setDeleteToggle}/></div>
                            <div className="col d-flex justify-content-end justify-content-md-start">
                                <Button className="mt-2 text-black refresh-btn"
                                        onClick={() => setMatchesToggle(!matchesToggle)}>
                                    <i className="bi-arrow-counterclockwise me-1"></i>
                                    Refresh list
                                </Button>
                            </div>
                        </>
                        : ''
                }
            </div>
        </div>
    );
};

export default MatchSection;

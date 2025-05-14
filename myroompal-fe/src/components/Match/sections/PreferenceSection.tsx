import {Button} from "react-bootstrap";
import React, {useEffect, useState} from "react";
import axios from "axios";
import {config} from "../../../config";
import TPreferenceResponseType from "../../../models/Match/PreferencesResponseType";

type TPreferenceSectionProps = {
    selectedPreferences: string[]
    setSelectedPreferences: React.Dispatch<React.SetStateAction<string[]>>
}
const PreferenceSection = ({selectedPreferences, setSelectedPreferences}: TPreferenceSectionProps) => {
    const [preferences, setPreferences] = useState<TPreferenceResponseType[]>()

    useEffect(() => {
        const url = `${config.apiBaseUrl}/matching/preferences`
        axios.get(url)
            .then(res => {
                setPreferences(res.data);
            })
            .catch(e => {
                console.log(e)
            })
    }, []);

    const PrefBtnOnClick = (id: string): void => {
        setSelectedPreferences((prevPreferences: string[]) => {
            const isSelected: boolean = prevPreferences.includes(id);
            if (isSelected) {
                return prevPreferences.filter((prefId) => prefId !== id);
            } else {
                return [...prevPreferences, id];
            }
        });
    }

    return (
        <div className="row bg-babyblue mt-5 g-0">
            <div className="px-5">
                <h2 className=" display-6 mt-5">Select your preferences</h2>
                <div className="mt-3 mb-5 row row-cols-1 row-cols-md-2 row-cols-lg-3 gx-4 gy-4">
                    {
                        preferences?.map((p: TPreferenceResponseType) => {
                            const isSelected: boolean = selectedPreferences.includes(p.id);
                            return (
                                <div key={p.id}>
                                    <Button
                                        className={`w-100 pt-2 px-3 d-flex justify-content-between align-items-center pref-btn
                                        ${isSelected ? "pref-btn-active" : "pref-btn-inactive"}`}
                                        onClick={() => PrefBtnOnClick(p.id)}
                                        cy-data={p.preferenceTag}
                                    >
                                        {p.preferenceTag}
                                        <i className={`bi-check-circle text-white g-0
                                        ${isSelected ? "" : "opacity-0"}`}
                                           style={{fontSize: "24px"}}>
                                        </i>
                                    </Button>
                                </div>
                            )
                        })
                    }
                </div>
            </div>
        </div>
    );
};

export default PreferenceSection;

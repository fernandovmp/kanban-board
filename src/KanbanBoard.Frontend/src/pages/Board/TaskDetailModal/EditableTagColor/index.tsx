import React, { useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import {
    apiPatch,
    isErrorResponse,
} from '../../../../services/kanbanApiService';
import { getJwtToken } from '../../../../services/tokenService';
import { Color, SaveButton, TagColorInput, TagColors } from './styles';

const colors = [
    '#FFEA31',
    '#FF3131',
    '#0069E4',
    '#00E409',
    '#8E00E4',
    '#9F9F9F',
];

interface IEditableTagColor {
    tagColor: string;
}

export const EditableTagColor: React.FC<IEditableTagColor> = ({ tagColor }) => {
    const [color, setColor] = useState('');
    const [selectedTagColor, setSelectedTagColor] = useState('');
    const history = useHistory();
    const { boardId, taskId } = useParams();

    useEffect(() => setSelectedTagColor(`#${tagColor}`), [tagColor]);
    useEffect(() => setColor(`#${tagColor}`), [tagColor]);

    const handleSave = async () => {
        const token = getJwtToken();
        const tagColorPatch = selectedTagColor.substring(1);
        const response = await apiPatch({
            uri: `v1/boards/${boardId}/tasks/${taskId}`,
            body: {
                tagColor: tagColorPatch,
            },
            bearerToken: token,
        });
        if (response.data && isErrorResponse(response.data)) {
            if (response.data.status === 401 || response.data.status === 403) {
                history.push('/login');
            }
            return;
        }
        setColor(selectedTagColor);
    };

    return (
        <>
            <TagColorInput
                value={selectedTagColor}
                onChange={(e) => setSelectedTagColor(e.target.value)}
            />
            <TagColors>
                {colors.map((color, index) => (
                    <Color
                        key={index}
                        color={color}
                        onClick={() => setSelectedTagColor(color)}
                    />
                ))}
            </TagColors>
            {selectedTagColor !== color && (
                <SaveButton onClick={handleSave}>SAVE</SaveButton>
            )}
        </>
    );
};

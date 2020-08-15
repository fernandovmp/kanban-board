import React, { useEffect, useState } from 'react';
import assignmentIcon from '../../../../assets/assignment.svg';
import deleteIcon from '../../../../assets/delete_outline.svg';
import { Task } from '../../../../models';
import {
    AssignedMemberName,
    AssignmentSectionTitle,
    Button,
    Color,
    SectionTitle,
    SidePanelWrapper,
    TagColorInput,
    TagColors,
} from './styles';

const colors = [
    '#FFEA31',
    '#FF3131',
    '#0069E4',
    '#00E409',
    '#8E00E4',
    '#9F9F9F',
];

interface ISidePanelProps {
    task?: Task;
}

export const SidePanel: React.FC<ISidePanelProps> = ({ task }) => {
    const [selectedTagColor, setSelectedTagColor] = useState('');

    useEffect(() => setSelectedTagColor(`#${task?.tagColor}`), [task]);

    return (
        <SidePanelWrapper>
            <SectionTitle>Tag</SectionTitle>
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
            <Button>
                <img src={deleteIcon} alt="Delete" /> DELETE TASK
            </Button>
            <AssignmentSectionTitle>
                <img src={assignmentIcon} alt="Assignments" /> Assignments
            </AssignmentSectionTitle>
            <Button>EDIT ASSIGNMENTS</Button>
            {task?.assignments.map((assignedMember, index) => (
                <AssignedMemberName key={index}>
                    {assignedMember.name}
                </AssignedMemberName>
            ))}
        </SidePanelWrapper>
    );
};

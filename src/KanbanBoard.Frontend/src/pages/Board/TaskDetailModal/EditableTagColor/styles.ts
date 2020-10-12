import styled from 'styled-components';
import { Input, PrimaryButton } from '../../../../components';

export const TagColorInput = styled(Input)`
    margin-top: 12px;
`;

export const TagColors = styled.ul`
    list-style: none;
    display: flex;
    flex-wrap: wrap;
    gap: 6px;
    padding: 0;
`;

interface IColorProps {
    color: string;
}

export const Color = styled.li<IColorProps>`
    width: 30px;
    height: 30px;
    background-color: ${(props) => props.color};
    border-radius: 15px;
`;

export const SaveButton = styled(PrimaryButton)`
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: small;
`;

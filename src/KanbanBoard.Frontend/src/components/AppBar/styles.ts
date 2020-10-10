import styled from 'styled-components';

export const AppBarHeader = styled.header`
    display: flex;
    background-color: var(--primary);
    align-items: center;
    justify-content: center;
    min-height: 50px;
    width: 100%;
    box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.25);
`;

interface IToolBarProps {
    align: 'left' | 'right';
}

export const ToolBar = styled.div<IToolBarProps>`
    flex: 1;
    display: flex;
    justify-content: ${(props) =>
        props.align === 'right' ? 'flex-end' : 'flex-start'};
    ${(props) => `margin-${props.align}`}: 24px;
`;

export const AppName = styled.strong`
    font-size: large;
`;

export const Icon = styled.img`
    :hover {
        cursor: pointer;
    }
`;

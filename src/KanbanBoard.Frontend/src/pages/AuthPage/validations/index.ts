import * as yup from 'yup';

export type LoginFormData = {
    email: string;
    password: string;
};

export type FormData = {
    name?: string;
    confirmPassword?: string;
} & LoginFormData;

export const loginSchema = yup.object().shape<LoginFormData>({
    email: yup
        .string()
        .required('e-mail is required')
        .email('insert a valid email'),
    password: yup.string().trim().required('password is required').min(8),
});

export const signUpSchema = yup.object().shape<FormData>({
    name: yup
        .string()
        .trim()
        .required('name is required')
        .max(100, "name length should be at maximum 100 character's"),
    email: yup
        .string()
        .required('e-mail is required')
        .email('insert a valid email'),
    password: yup.string().trim().required().min(8),
    confirmPassword: yup
        .string()
        .trim()
        .required()
        .min(8)
        .equals([yup.ref('password')], "passwords doesn't match"),
});

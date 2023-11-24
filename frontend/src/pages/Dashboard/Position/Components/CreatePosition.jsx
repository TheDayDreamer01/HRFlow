import * as Yup from "yup";
import { useFormik } from "formik";
import { ModalBox } from "/src/components/ModalBox";
import { TextInput, SubmitButton } from "/src/components/FormInput";
import { useNavigate } from "react-router-dom";
import { CircularProgressBar } from "/src/components/Loading";
import { AddPositionApi, UpdatePositionPropertyApi } from "../../../../services/positionService";
import Toast from "/src/components/Toast";
import useToggle from "/src/hooks/useToggle";

const CreatePosition = (prop) => {
    const token = sessionStorage.getItem("token");

    const navigate = useNavigate();

    const [toast, onSetToast] = useToggle();

    const formik = useFormik({
        initialValues: {
            title: prop.addPosition ? "" : prop.selectedPosition.title
        },

        onSubmit: async (values) => {
            prop.onSetSubmit();

            const response = prop.addPosition
                ? await AddPositionApi(token, prop.selectedDepartment.id, values)
                : await UpdatePositionPropertyApi(
                    token,
                    prop.selectedDepartment.id,
                    prop.selectedPosition.id,
                    [{
                        path: "/title",
                        op: "replace",
                        value: values.title
                    }]
                );

            setTimeout(() => {
                if (response.status === 200) {
                    onSetToast();
                    setTimeout(() => {
                        prop.onSetOpenModal();
                    }, 800);
                } else if (response.status === 400) {
                    formik.setErrors({
                        title: response.data
                    });
                } else {
                    navigate("/error");
                }

                prop.onSetPositionSubmit();
                prop.onSetSubmit();
            }, 800);
        },

        validationSchema: Yup.object({
            title: Yup.string().required("Position Title is required.")
                .min(2, "Position Title must be at least 2 characters.")
                .max(50, "Position Title can be at most 50 characters.")
        })
    });

    return (
        <ModalBox top="mt-8" onCancel={prop.onCancel}>
            {toast && <Toast message={`Position successfully ${prop.addPosition ? "created!" : "updated!"}`} />}
            <header className="mb-6">
                <span className="flex items-center gap-2">
                    <h1 className="text-2xl sm:text-3xl font-lato font-bold text-lilac ">
                        {prop.addPosition ? "Add" : "Update"} Position
                    </h1>
                </span>
            </header>
            <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 w-full">
                <TextInput nameId="title"
                    name="Title"
                    type="text"
                    placeholder="Manager"
                    minLength={2}
                    maxLength={50}
                    onBlur={formik.handleBlur}
                    errors={formik.errors.title}
                    touched={formik.touched.title}
                    onChange={formik.handleChange}
                    value={formik.values.title} />

                <div className="self-end w-1/2 flex gap-2">
                    <button className="bg-gray-200 hover:bg-gray-100 rounded-full h-14 font-semibold w-full shadow-lg">
                        <h1 onClick={prop.onCancel}
                            className="w-full h-full flex items-center justify-center">
                            Cancel
                        </h1>
                    </button>
                    <SubmitButton>
                        {(prop.submit) ? (
                            <CircularProgressBar>
                                <p className="ml-2 text-poppins text-white">Loading...</p>
                            </CircularProgressBar>
                        ) : (
                            <p className="text-poppins text-white">
                                {prop.addPosition ? "Submit" : "Update"}
                            </p>
                        )}
                    </SubmitButton>
                </div>
            </form>
        </ModalBox>
    );
}

export default CreatePosition;

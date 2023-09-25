import React, { forwardRef } from "react";
import Bell from "assets/svg/icons/Bell.svg";
import Clock from "assets/svg/icons/Clock.svg";
import Coin from "assets/svg/icons/Coin.svg";
import Folder from "assets/svg/icons/Folder.svg";
import Megaphone from "assets/svg/icons/Megaphone.svg";
import Person from "assets/svg/icons/Person.svg";
import Diamond from "assets/svg/icons/Diamond.svg";
import FeatureCard from "../components/FeatureCard";
import PurpleGradient from "assets/svg/Services_purple.svg";
import PinkGradient from "assets/svg/Services_pink.svg";

const Services = forwardRef((props, ref) => {
    return (
        <section ref={ref} className="relative flex flex-col px-4 justify-center scroll-m-32">
            <img
                src={PurpleGradient}
                className="absolute -z-10 top-24 left-16 md:-right-10 md:top-32 xl:left-60 xl:top-28"
            />
            <img
                src={PinkGradient}
                className="absolute -z-20 top-10 -left-28 md:top-0 md:-left-32 xl:-left-10 xl:-top-6"
            />
            <div className="w-full max-w-screen-2xl mx-auto">
                <div className="pt-2 pb-8">
                    <img src={Diamond} className="h-20"/>
                    <h1 className="transition leading-none text-jetblack font-lato font-black text-5xl md:text-5xl py-6 pb-2 pr-10">
                        Seamless HR Solutions
                    </h1>
                    <p className="leading-tight text-gray-700 font-poppins text-xl md:text-[1.2rem] py-2 2xl:text-2xl transition">
                        HR Flow offers variety of services for the ease of use of both HR professionals and employees.
                    </p>
                </div>
                <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
                    <FeatureCard
                        featureIcon={Folder}
                        featureTitle="Employee Records Management"
                        featureDescription="Maintain a comprehensive database of employee information, including personal details and  contact information."
                    />
                    <FeatureCard
                        featureIcon={Person}
                        featureTitle="Employee Self-service"
                        featureDescription="Allow employees to update their personal information and submit requests and documents."
                    />
                    <FeatureCard
                        featureIcon={Coin}
                        featureTitle="Payroll, Benefits, and Compensation Management"
                        featureDescription="Automate payroll calculations, tax deductions, and generate paychecks, manages employee benefits and salary structures."
                    />
                    <FeatureCard
                        featureIcon={Clock}
                        featureTitle="Time and Attendance Tracking"
                        featureDescription="Record and monitor employee attendance and work hours, including clock-in/clock-out features."
                    />
                    <FeatureCard
                        featureIcon={Megaphone}
                        featureTitle="Recruitment and Applicant Tracking"
                        featureDescription="Accept applications and track candidates throughout the hiring process."
                    />
                    <FeatureCard
                        featureIcon={Bell}
                        featureTitle="Notifications and Alerts"
                        featureDescription="Send automated notifications for important announcements and upcoming events to employees."
                    />
                </div>
            </div>
            
        </section>
    );
});

export default Services;
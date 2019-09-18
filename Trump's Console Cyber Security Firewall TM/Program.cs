using System;
using System.Drawing;
using System.Diagnostics;
using static Trump_s_Console_Cyber_Security_Firewall_TM.Label;
using static Trump_s_Console_Cyber_Security_Firewall_TM.MenuItem;
using static Trump_s_Console_Cyber_Security_Firewall_TM.Screen;
using System.Text;

namespace Trump_s_Console_Cyber_Security_Firewall_TM
{
    class Program
    {
        static private Screen MyScreen;
        static private Menu MainMenu, ConfigMenu;
        private static readonly ConsoleColor StartColor = Console.BackgroundColor;

        static void Main(string[] args)
        {
            MainMenu = new Menu("Main", Color.DarkRed);
            MainMenu.Add(new Label("Bazinga", AnchorSide.Left | AnchorSide.Top, 10, 4, Color.Black));
            //MainMenu.Add(new Button("Button 1", AnchorSide.Right | AnchorSide.Top, 20, 10, Color.BlueViolet));

            ConfigMenu = new Menu("Config",Color.DarkGreen);
            ConfigMenu.Add(new Label("Kachow", AnchorSide.Right | AnchorSide.Bottom, 10, 4));

            MyScreen = new Screen(MainMenu);
            MyScreen.AddMenu(ConfigMenu);

            MyScreen.WindowResizedEvent += OnWindowResized;
            MyScreen.KeyReceivedEvent += OnKeyReceived;

            Secure();
        }

        static void OnWindowResized(object sender, EventArgs e)
        {
        }

        static void OnKeyReceived(object sender, KeyReceivedArgs e)
        {
            switch (e.KeyInfo.Key)
            {
                case (ConsoleKey.Q):
                    Quit();
                    break;
            }
        }

        static string ReplaceStringInFile(string filepath, string toReplace, string replacement)
        {
            return $"sudo sed -i '.bak' -e 's/{toReplace}/{replacement}/g' {filepath}".Bash();
        }

        static string AddStringsToFile(string filepath, params string[] newText)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string text in newText)
                sb.Append($"sudo echo -e \"\n{text}\" >> {filepath}".Bash());
            return sb.ToString();
        }

        static string SetFileParameters(string filepath, string delimiter, params string[] newParam)
        {
            StringBuilder sb = new StringBuilder();
            if (delimiter.Equals(' '))
                delimiter = "[[:space:]]";
            foreach (string text in newParam)
            {
                string paramName = text.Split(delimiter)[0].Trim();
                string paramValue = text.Split(delimiter)[1].Trim();
                if ($"sudo grep \"^[^#;]{paramName}\" \"{filepath}\"".Bash().Trim().Equals(""))
                    AddStringsToFile(filepath, text);
                else
                    sb.Append($"sudo sed -e '/#/!s/\\({paramName}[[:space:]]*{delimiter}[[:space:]]*\\)\\(.*\\)/\\1\"{paramValue}\"/' {filepath}".Bash());
            }
            return sb.ToString();
        }

        static string InstallPackages(params string[] names)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string name in names)
                sb.Append(name + " ");
            return $"sudo apt-get install {sb.ToString()}".Bash();
        }

        static string RemovePackages(params string[] names)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string name in names)
                sb.Append(name + " ");
            return $"sudo apt-get remove {sb.ToString()}".Bash();
        }
        static string DisableSysCtl(params string[] names)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string name in names)
                sb.Append($"sudo systemctl disable {name}".Bash());
            return sb.ToString();
        }

        static void Secure()
        {
            "sudo apt-get update".Bash();
            InstallPackages("aide", "aide-common", "selinux", "apparmor", "ntp", "chrony", 
                "tcpd", "iptables", "rsyslog", "syslog-ng", "libpam-pwquality");
            AddStringsToFile("/etc/modprobe.d/CIS.conf", 
                "install cramfs /bin/true",
                "install freevxfs /bin/true",
                "install jffs2 /bin/true",
                "install hfs /bin/true",
                "install hfsplus /bin/true",
                "install udf /bin/true",
                "install dccp /bin/true",
                "install sctp /bin/true",
                "install rds /bin/true",
                "install tipc /bin/true"
                );
            "sudo rmmod cramfs".Bash();
            "sudo rmmod freevxfs".Bash();
            "sudo rmmod jffs2".Bash();
            "sudo rmmod hfs".Bash();
            "sudo rmmod hfsplus".Bash();
            "sudo rmmod udf".Bash();
            //Skipped "Ensure separate partition exists for /tmp"
            //Skipped "Ensure nodev option set on /tmp partition"
            //Skipped "Ensure nosuid option set on /tmp partition"
            //Skipped "Ensure separate partition exists for /var"
            //Skipped "Ensure separate partition exists for /var/tmp"
            //Skipped "Ensure nodev option set on /var/tmp partition"
            //Skipped "Ensure nosuid option set on /var/tmp partition"
            //Skipped "Ensure noexec option set on /var/tmp partition"
            //Skipped "Ensure separate partition exists for /var/log"
            //Skipped "Ensure separate partition exists for /var/log/audit"
            //Skipped "Ensure separate partition exists for /home"
            //Skipped "Ensure nodev option set on /home partition"
            //Skipped "Ensure nodev option set on /dev/shm partition"
            //Skipped "Ensure nosuid option set on /dev/shm partition"
            //Skipped "Ensure noexec option set on /dev/shm partition"
            //Skipped "Ensure nodev option set on removable media partitions"
            //Skipped "Ensure nosuid option set on removable media partitions"
            //Skipped "Ensure noexec option set on removable media partitions"
            "sudo df --local -P | awk {'if (NR!=1) print $6'} | xargs -I '{}' find '{}' -xdev -type d -perm -0002 2>/dev/null | xargs chmod a+t".Bash();
            //Skipped "Configure Software Updates"
            //Skipped "Ensure GPG keys are configured"
            "sudo aideinit".Bash();
            "sudo crontab -u root -l > temp-crontab".Bash();
            AddStringsToFile("temp-crontab", "0 5 * * * /usr/bin/aide --config /etc/aide/aide.conf --check");
            "sudo crontab -u root temp-crontab".Bash();
            "sudo chown root:root /boot/grub/grub.cfg".Bash();
            "sudo chmod og-rwx /boot/grub/grub.cfg".Bash();
            //Skipped "Ensure bootloader password is set"
            "sudo echo $uperSuit76 | passwd --stdin root".Bash(); //TODO: FIX
            AddStringsToFile("/etc/security/limits.conf", "* hard core 0");
            SetFileParameters("/etc/sysctl.conf", "=",
                "fs.suid_dumpable = 0",
                "kernel.randomize_va_space = 2",
                "net.ipv4.ip_forward = 0",
                "net.ipv4.conf.all.send_redirects = 0",
                "net.ipv4.conf.default.send_redirects = 0",
                "net.ipv4.conf.all.accept_source_route = 0",
                "net.ipv4.conf.default.accept_source_route = 0",
                "net.ipv4.conf.all.accept_redirects = 0",
                "net.ipv4.conf.default.accept_redirects = 0",
                "net.ipv4.conf.all.secure_redirects = 0",
                "net.ipv4.conf.default.secure_redirects = 0",
                "net.ipv4.conf.all.log_martians = 1",
                "net.ipv4.conf.default.log_martians = 1",
                "net.ipv4.icmp_echo_ignore_broadcasts = 1",
                "net.ipv4.icmp_ignore_bogus_error_responses = 1",
                "net.ipv4.conf.all.rp_filter = 1",
                "net.ipv4.conf.default.rp_filter = 1",
                "net.ipv4.tcp_syncookies = 1",
                "net.ipv6.conf.all.accept_ra = 0",
                "net.ipv6.conf.default.accept_ra = 0",
                "net.ipv6.conf.all.accept_redirects = 0",
                "net.ipv6.conf.default.accept_redirects = 0"
                );
            "sudo sysctl -w fs.suid_dumpable=0".Bash();
            "sudo sysctl -w net.ipv4.ip_forward=0".Bash();
            "sudo sysctl -w net.ipv4.route.flush=1".Bash();
            "sudo sysctl -w net.ipv4.conf.all.send_redirects=0".Bash();
            "sudo sysctl -w net.ipv4.conf.default.send_redirects=0".Bash();
            "sudo sysctl -w net.ipv4.route.flush=1".Bash();
            "sudo sysctl -w net.ipv4.conf.all.accept_source_route=0".Bash();
            "sudo sysctl -w net.ipv4.conf.default.accept_source_route=0".Bash();
            "sudo sysctl -w net.ipv4.route.flush=1".Bash();
            "sudo sysctl -w net.ipv4.conf.all.accept_redirects=0".Bash();
            "sudo sysctl -w net.ipv4.conf.default.accept_redirects=0".Bash();
            "sudo sysctl -w net.ipv4.route.flush=1".Bash();
            "sudo sysctl -w net.ipv4.conf.all.secure_redirects=0".Bash();
            "sudo sysctl -w net.ipv4.conf.default.secure_redirects=0".Bash();
            "sudo sysctl -w net.ipv4.route.flush=1".Bash();
            "sudo sysctl -w net.ipv4.conf.all.log_martians=1".Bash();
            "sudo sysctl -w net.ipv4.conf.default.log_martians=1".Bash();
            "sudo sysctl -w net.ipv4.route.flush=1".Bash();
            "sudo net.ipv4.icmp_echo_ignore_broadcasts = 1".Bash();
            "sudo sysctl -w net.ipv4.route.flush=1".Bash();
            "sudo sysctl -w net.ipv4.icmp_ignore_bogus_error_responses=1".Bash();
            "sudo sysctl -w net.ipv4.route.flush=1".Bash();
            "sudo sysctl -w net.ipv4.conf.all.rp_filter=1".Bash();
            "sudo sysctl -w net.ipv4.conf.default.rp_filter=1".Bash();
            "sudo sysctl -w net.ipv4.route.flush=1".Bash();
            "sudo sysctl -w net.ipv4.tcp_syncookies=1".Bash();
            "sudo sysctl -w net.ipv4.route.flush=1".Bash();
            "sudo sysctl -w net.ipv6.conf.all.accept_ra=0".Bash();
            "sudo sysctl -w net.ipv6.conf.default.accept_ra=0".Bash();
            "sudo sysctl -w net.ipv6.route.flush=1".Bash();
            "sudo sysctl -w net.ipv6.conf.all.accept_redirects=0".Bash();
            "sudo sysctl -w net.ipv6.conf.default.accept_redirects=0".Bash();
            "sudo sysctl -w net.ipv6.route.flush=1".Bash();
            //Skipped "Ensure XD/NX support is enabled"
            "sudo sysctl -w kernel.randomize_va_space=2".Bash();
            "sudo prelink -ua".Bash();
            ReplaceStringInFile("/etc/default/grub", "selinux=0","");
            ReplaceStringInFile("/etc/default/grub", "enforcing=0", "");
            "sudo update-grub".Bash();
            SetFileParameters("/etc/selinux/configfile","=", 
                "SELINUX=enforcing",
                "SELINUXTYPE=ubuntu"
                );
            //Skipped "Ensure no unconfined daemons exist"
            ReplaceStringInFile("/etc/selinux/configfile", "apparmor=0", "");
            "sudo update-grub".Bash();
            "sudo aa-enforce /etc/apparmor.d/*".Bash();
            ReplaceStringInFile("/etc/motd", "\\m", "");
            ReplaceStringInFile("/etc/motd", "\\r", "");
            ReplaceStringInFile("/etc/motd", "\\s", "");
            ReplaceStringInFile("/etc/motd", "\\v", "");
            ReplaceStringInFile("/etc/issue", "\\m", "");
            ReplaceStringInFile("/etc/issue", "\\r", "");
            ReplaceStringInFile("/etc/issue", "\\s", "");
            ReplaceStringInFile("/etc/issue", "\\v", "");
            "sudo echo \"Authorized uses only.All activity may be monitored and reported.\" > /etc/issue".Bash();
            ReplaceStringInFile("/etc/issue.net", "\\m", "");
            ReplaceStringInFile("/etc/issue.net", "\\r", "");
            ReplaceStringInFile("/etc/issue.net", "\\s", "");
            ReplaceStringInFile("/etc/issue.net", "\\v", "");
            "sudo echo \"Authorized uses only.All activity may be monitored and reported.\" > /etc/issue.net".Bash();
            "sudo chown root:root /etc/motd".Bash();
            "sudo chmod 644 /etc/motd".Bash();
            "sudo chown root:root /etc/issue".Bash();
            "sudo chmod 644 /etc/issue".Bash();
            "sudo chown root:root /etc/issue.net".Bash();
            "sudo chmod 644 /etc/issue.net".Bash();
            AddStringsToFile("/etc/dconf/profile/gdm", 
                "user-db:user",
                "system-db:gdm file-db:/usr/share/gdm/greeter-dconf-defaults"
                );
            AddStringsToFile("/etc/dconf/db/gdm.d/01-banner-message", 
                "[org/gnome/login-screen]",
                "banner-message-enable=true",
                "banner-message-text='Authorized uses only. All activity may be monitored and reported.'"
                );
            "sudo dconf update".Bash();
            //Skipped "Ensure chargen services are not enabled"
            //Skipped "Ensure daytime services are not enabled"
            //Skipped "Ensure discard services are not enabled"
            //Skipped "Ensure echo services are not enabled"
            //Skipped "Ensure time services are not enabled"
            //Skipped "Ensure rsh server is not enabled"
            //Skipped "Ensure talk server is not enabled"
            //Skipped "Ensure telnet server is not enabled"
            //Skipped "Ensure tftp server is not enabled"
            AddStringsToFile("/etc/ntp.conf", 
                "restrict -4 default kod nomodify notrap nopeer noquery",
                "restrict -6 default kod nomodify notrap nopeer noquery",
                "server <remote-server>"
                );
            AddStringsToFile("/etc/init.d/ntp", "RUNASUSER=ntp");
            //Skipped "Ensure chrony is configured"
            DisableSysCtl("avahi-daemon", "cups", "isc-dhcp-server", 
                "isc-dhcp-server6", "slapd", "nfs-server", "rpcbind",
                "bind9", "vsftpd", "apache2", "dovecot", "smbd", 
                "squid", "snmpd", "rsync", "nis", "xinetd", "autof");
            AddStringsToFile("/etc/postfix/main.cf", "inet_interfaces = loopback-only");
            "sudo systemctl restart postfix".Bash();
            AddStringsToFile("/etc/default/grub", 
                "GRUB_CMDLINE_LINUX=\"ipv6.disable = 1\"",
                "GRUB_CMDLINE_LINUX=\"audit = 1\"");
            "sudo echo \"ALL: < net >/< mask >, < net >/< mask >, ...\" >/etc/hosts.allow".Bash();
            "sudo echo \"ALL: ALL\" >> /etc/hosts.deny".Bash();
            "sudo chown root:root /etc/hosts.allow".Bash();
            "sudo chmod 644 /etc/hosts.allow".Bash();
            "sudo chown root:root /etc/hosts.deny".Bash();
            "sudo chmod 644 /etc/hosts.deny".Bash();
            "sudo iptables -P INPUT DROP".Bash();
            "sudo iptables -P OUTPUT DROP".Bash();
            "sudo iptables -P FORWARD DROP".Bash();
            "sudo iptables -A INPUT -i lo -j ACCEPT".Bash();
            "sudo iptables -A OUTPUT -o lo -j ACCEPT".Bash();
            "sudo iptables -A INPUT -s 127.0.0.0/8 -j DROP".Bash();
            "sudo iptables -A OUTPUT -p tcp -m state --state NEW,ESTABLISHED -j ACCEPT".Bash();
            "sudo iptables -A OUTPUT -p udp -m state --state NEW,ESTABLISHED -j ACCEPT".Bash();
            "sudo iptables -A OUTPUT -p icmp -m state --state NEW,ESTABLISHED -j ACCEPT".Bash();
            "sudo iptables -A INPUT -p tcp -m state --state ESTABLISHED -j ACCEPT".Bash();
            "sudo iptables -A INPUT -p udp -m state --state ESTABLISHED -j ACCEPT".Bash();
            "sudo iptables -A INPUT -p icmp -m state --state ESTABLISHED -j ACCEPT".Bash();
            //Skipped "Ensure firewall rules exist for all open ports"
            //Skipped "Ensure wireless interfaces are disabled"
            //Skipped "Ensure audit log storage size is configured"
            SetFileParameters("/etc/audit/auditd.conf", "=",
                "space_left_action = email",
                "action_mail_acct = root",
                "admin_space_left_action = halt",
                "max_log_file_action = keep_logs"
                );
            "sudo systemctl enable auditd".Bash();
            "sudo update-grub".Bash();
            AddStringsToFile("/etc/audit/audit.rules",
                "-a always,exit -F arch=b64 -S adjtimex -S settimeofday -k time-change",
                "-a always,exit -F arch=b32 -S adjtimex -S settimeofday -S stime -k timechange",
                "-a always,exit -F arch=b64 -S clock_settime -k time-change",
                "-a always,exit -F arch=b32 -S clock_settime -k time-change",
                "-w /etc/localtime -p wa -k time-change",
                "-w /etc/group -p wa -k identity",
                "-w /etc/passwd -p wa -k identity",
                "-w /etc/gshadow -p wa -k identity",
                "-w /etc/shadow -p wa -k identity",
                "-w /etc/security/opasswd -p wa -k identity",
                "-a always,exit -F arch=b64 -S sethostname -S setdomainname -k system-locale",
                "-a always,exit -F arch=b32 -S sethostname -S setdomainname -k system-locale",
                "-w /etc/issue -p wa -k system-locale",
                "-w /etc/issue.net -p wa -k system-locale",
                "-w /etc/hosts -p wa -k system-locale",
                "-w /etc/sysconfig/network -p wa -k system-locale",
                "-w /etc/selinux/ -p wa -k MAC-policy",
                "-w /usr/share/selinux/ -p wa -k MAC-policy",
                "-w /etc/apparmor/ -p wa -k MAC-policy",
                "-w /etc/apparmor.d/ -p wa -k MAC-policy",
                "-w /var/log/faillog -p wa -k logins",
                "-w /var/log/lastlog -p wa -k logins",
                "-w /var/log/tallylog -p wa -k logins",
                "-w /var/run/utmp -p wa -k session",
                "-w /var/log/wtmp -p wa -k logins",
                "-w /var/log/btmp -p wa -k logins",
                "-a always,exit -F arch=b64 -S chmod -S fchmod -S fchmodat -F auid>=1000 -F auid!=4294967295 -k perm_mod",
                "-a always,exit -F arch=b32 -S chmod -S fchmod -S fchmodat -F auid>=1000 -F auid!=4294967295 -k perm_mod",
                "-a always,exit -F arch=b64 -S chown -S fchown -S fchownat -S lchown -F auid>=1000 -F auid!=4294967295 -k perm_mod",
                "-a always,exit -F arch=b32 -S chown -S fchown -S fchownat -S lchown -F auid>=1000 -F auid!=4294967295 -k perm_mod",
                "-a always,exit -F arch=b64 -S setxattr -S lsetxattr -S fsetxattr -S removexattr -S lremovexattr -S fremovexattr -F auid>=1000 -F auid!=4294967295 -k perm_mod",
                "-a always,exit -F arch=b32 -S setxattr -S lsetxattr -S fsetxattr -S removexattr -S lremovexattr -S fremovexattr -F auid>=1000 -F auid!=4294967295 -k perm_mod",
                "-a always,exit -F arch=b64 -S creat -S open -S openat -S truncate -S ftruncate -F exit=-EACCES -F auid>=1000 -F auid!=4294967295 -k access",
                "-a always,exit -F arch=b32 -S creat -S open -S openat -S truncate -S ftruncate -F exit=-EACCES -F auid>=1000 -F auid!=4294967295 -k access",
                "-a always,exit -F arch=b64 -S creat -S open -S openat -S truncate -S ftruncate -F exit=-EPERM -F auid>=1000 -F auid!=4294967295 -k access",
                "-a always,exit -F arch=b32 -S creat -S open -S openat -S truncate -S ftruncate -F exit=-EPERM -F auid>=1000 -F auid!=4294967295 -k access",
                "-a always,exit -F arch=b64 -S mount -F auid>=1000 -F auid!=4294967295 -k mounts",
                "-a always,exit -F arch=b32 -S mount -F auid>=1000 -F auid!=4294967295 -k mounts",
                "-a always,exit -F arch=b64 -S unlink -S unlinkat -S rename -S renameat -F auid>=1000 -F auid!=4294967295 -k delete",
                "-a always,exit -F arch=b32 -S unlink -S unlinkat -S rename -S renameat -F auid>=1000 -F auid!=4294967295 -k delete",
                "-w /etc/sudoers -p wa -k scope",
                "-w /etc/sudoers.d/ -p wa -k scope",
                "-w /var/log/sudo.log -p wa -k actions",
                "-w /sbin/insmod -p x -k modules",
                "-w /sbin/rmmod -p x -k modules",
                "-w /sbin/modprobe -p x -k modules",
                "-a always,exit -F arch=b64 -S init_module -S delete_module -k modules",
                "-e 2"
                );
            //Skipped "Ensure use of privileged commands is collected"
            "sudo systemctl enable rsyslog".Bash();
            AddStringsToFile("/etc/rsyslog.conf",
                "*.emerg :omusrmsg:*",
                "mail.* -/var/log/mail",
                "mail.info -/var/log/mail.info",
                "mail.warning -/var/log/mail.warn",
                "mail.err /var/log/mail.err",
                "news.crit -/var/log/news/news.crit",
                "news.err -/var/log/news/news.err",
                "news.notice -/var/log/news/news.notice",
                "*.=warning;*.=err -/var/log/warn",
                "*.crit /var/log/warn",
                "*.*;mail.none;news.none -/var/log/messages",
                "local0,local1.* -/var/log/localmessages",
                "local2,local3.* -/var/log/localmessages",
                "local4,local5.* -/var/log/localmessages",
                "local6,local7.* -/var/log/localmessages",
                "$FileCreateMode 0640"
                );
            "sudo pkill -HUP rsyslogd".Bash();
            //Skipped "Ensure rsyslog is configured to send logs to a remote log host"
            //Skipped " Ensure remote rsyslog messages are only accepted on designated log hosts"
            "sudo update-rc.d syslog-ng enable".Bash();
            AddStringsToFile("/etc/syslog-ng/syslog-ng.conf",
                "log { source(src); source(chroots); filter(f_console); destination(console);};",
                "log { source(src); source(chroots); filter(f_console); destination(xconsole);};",
                "log { source(src); source(chroots); filter(f_newscrit); destination(newscrit); };",
                "log { source(src); source(chroots); filter(f_newserr); destination(newserr);};",
                "log { source(src); source(chroots); filter(f_newsnotice); destination(newsnotice);};",
                "log { source(src); source(chroots); filter(f_mailinfo); destination(mailinfo);};",
                "log { source(src); source(chroots); filter(f_mailwarn); destination(mailwarn);};",
                "log { source(src); source(chroots); filter(f_mailerr); destination(mailerr);};",
                "log { source(src); source(chroots); filter(f_mail); destination(mail);};",
                "log { source(src); source(chroots); filter(f_acpid); destination(acpid); flags(final);};",
                "log { source(src); source(chroots); filter(f_acpid_full); destination(devnull); flags(final);};",
                "log { source(src); source(chroots); filter(f_acpid_old); destination(acpid); flags(final);};",
                "log { source(src); source(chroots); filter(f_netmgm); destination(netmgm);flags(final);};",
                "log { source(src); source(chroots); filter(f_local); destination(localmessages);};",
                "log { source(src); source(chroots); filter(f_messages); destination(messages);};",
                "log { source(src); source(chroots); filter(f_iptables); destination(firewall);};",
                "log { source(src); source(chroots); filter(f_warn); destination(warn);};",
                "options { chain_hostnames(off); flush_lines(0); perm(0640); stats_freq(3600);threaded(yes);};"
                );
            //Skipped "Ensure syslog-ng is configured to send logs to a remote log host"
            //Skipped "Ensure remote syslog-ng messages are only accepted on designated log hosts"
            "sudo chmod -R g-wx,o-rwx /var/log/*".Bash();
            //Skipped "Ensure logrotate is configured"
            "sudo systemctl enable cron".Bash();
            "sudo chown root:root /etc/crontab".Bash();
            "sudo chmod og-rwx /etc/crontab".Bash();
            "sudo chown root:root /etc/cron.hourly".Bash();
            "sudo chmod og-rwx /etc/cron.hourly".Bash();
            "sudo chown root:root /etc/cron.daily".Bash();
            "sudo chmod og-rwx /etc/cron.daily".Bash();
            "sudo chown root:root /etc/cron.weekly".Bash();
            "sudo chmod og-rwx /etc/cron.weekly".Bash();
            "sudo chown root:root /etc/cron.monthly".Bash();
            "sudo chmod og-rwx /etc/cron.monthly".Bash();
            "sudo chown root:root /etc/cron.d".Bash();
            "sudo chmod og-rwx /etc/cron.d".Bash();
            "sudo rm /etc/cron.deny".Bash();
            "sudo rm /etc/at.deny".Bash();
            "sudo touch /etc/cron.allow".Bash();
            "sudo touch /etc/at.allow".Bash();
            "sudo chmod og-rwx /etc/cron.allow".Bash();
            "sudo chmod og-rwx /etc/at.allow".Bash();
            "sudo chown root:root /etc/cron.allow".Bash();
            "sudo chown root:root /etc/at.allow".Bash();
            "sudo chown root:root /etc/ssh/sshd_config".Bash();
            SetFileParameters("/etc/ssh/sshd_config", " ",
                "Protocol 2",
                "LogLevel INFO",
                "X11Forwarding no",
                "MaxAuthTries 4",
                "IgnoreRhosts yes",
                "HostbasedAuthentication no",
                "PermitRootLogin no",
                "PermitEmptyPasswords no",
                "PermitUserEnvironment no",
                "MACs hmac-sha2-512-etm@openssh.com,hmac-sha2-256-etm@openssh.com,umac-128-etm@openssh.com,hmac-sha2-512,hmac-sha2-256,umac-128@openssh.com",
                "ClientAliveInterval 300",
                "ClientAliveCountMax 0",
                "LoginGraceTime 60",
                "Banner /etc/issue.net"
                );
            //Skipped "Ensure SSH access is limited"
            AddStringsToFile("/etc/pam.d/common-password",
                "password requisite pam_pwquality.so retry=3",
                "password required pam_pwhistory.so remember=5",
                "password [success=1 default=ignore] pam_unix.so sha512");
            SetFileParameters("/etc/security/pwquality.conf","=",
                "minlen = 14",
                "dcredit = -1",
                "ucredit = -1",
                "ocredit = -1",
                "lcredit = -1"
                );
            AddStringsToFile("/etc/pam.d/common-authfile",
                "auth required pam_tally2.so onerr=fail audit silent deny=5 unlock_time=900");
            SetFileParameters("/etc/login.defs", " ",
                "PASS_MAX_DAYS 90",
                "PASS_MIN_DAYS 7",
                "PASS_WARN_AGE 7");
            //Skipped "Ensure password expiration is 365 days or less"
            //Skipped "Ensure minimum days between password changes is 7 or more"
            //Skipped "Ensure password expiration warning days is 7 or more"
            "sudo useradd -D -f 30".Bash();
            //Skipped "Ensure inactive password lock is 30 days or less"
            //Skipped "Ensure all users last password change date is in the past"
            "for user in `awk -F: '($3 < 1000) {print $1 }' /etc/passwd`; do; if [ $user != \"root\" ]; then; sudo usermod -L $user; if [ $user != \"sync\" ] && [ $user != \"shutdown\" ] && [ $user != \"halt\" ];; then; sudo usermod -s /usr/sbin/nologin $user; fi; fi; done".Bash();
            "sudo usermod -g 0 root".Bash();
            //Skipped "Ensure default user umask is 027 or more restrictive"
            //Skipped "Ensure default user shell timeout is 900 seconds or less"
            //Skipped "Ensure root login is restricted to system console"
            AddStringsToFile("/etc/pam.d/su", "auth required pam_wheel.so");
            //Skipped "Ensure access to the su command is restricted"
            //Skipped "Audit system file permissions"
            "sudo chown root:root /etc/passwd".Bash();
            "sudo chmod 644 /etc/passwd".Bash();
            "sudo chown root:shadow /etc/shadow".Bash();
            "sudo chmod o-rwx,g-wx /etc/shadow".Bash();
            "sudo chown root:root /etc/group".Bash();
            "sudo chmod 644 /etc/group".Bash();
            "sudo chown root:shadow /etc/gshadow".Bash();
            "sudo chmod o-rwx,g-rw /etc/gshadow".Bash();
            "sudo chown root:root /etc/passwd-".Bash();
            "sudo chown root:root /etc/shadow-".Bash();
            "sudo chown root:shadow /etc/shadow-".Bash();
            "sudo chmod o-rwx,g-rw /etc/shadow-".Bash();
            "sudo chown root:root /etc/group-".Bash();
            "sudo chmod u-x,go-wx /etc/group-".Bash();
            "sudo chown root:root /etc/gshadow-".Bash();
            "sudo chown root:shadow /etc/gshadow-".Bash();
            "sudo chmod o-rwx,g-rw /etc/gshadow-".Bash();
            //Skipped "Ensure no world writable files exist"
            //Skipped "Ensure no unowned files or directories exist"
            //Skipped "Ensure no ungrouped files or directories exist"
            //Skipped "Audit SUID executables"
            //Skipped "Audit SGID executables"
            //Skipped "Ensure password fields are not empty"
            //Skipped "Ensure no legacy "+" entries exist in /etc/passwd"
            //Skipped "Ensure no legacy "+" entries exist in /etc/shadow"
            //Skipped "Ensure no legacy "+" entries exist in /etc/group"
            //Skipped "Ensure root is the only UID 0 account"
            //Skipped "Ensure root PATH Integrity"
            //SKipped "Ensure all users' home directories exist"
            //Skipped "Ensure users' home directories permissions are 750 or more restrictive"
            //Skipped "Ensure users own their home directories"
            //Skipped "Ensure users' dot files are not group or world writable"
            //Skipped "Ensure no users have .forward files"
            //Skipped "Ensure no users have .netrc files"
            //Skipped "Ensure users' .netrc Files are not group or world accessible"
            //Skipped "Ensure no users have .rhosts files"
            //Skipped "Ensure all groups in /etc/passwd exist in /etc/group"
            //Skipped "Ensure no duplicate UIDs exist"
            //Skipped "Ensure no duplicate GIDs exist"
            //Skipped "Ensure no duplicate user names exist"
            //Skipped "Ensure no duplicate group names exist"
            //Skipped "Ensure shadow group is empty"

            RemovePackages("prelink", "openbsd-inetd", "xserver-xorg*", "nis", "rsh-client",
                "rsh-redone-client", "talk", "telnet", "ldap-utils");

            "sudo apt-get upgrade".Bash();
            
            //string file = "/etc/ssh/sshd_config";
            //ReplaceStringInFile(file, "Port 22", "Port 2020");
            //ReplaceStringInFile(file, "Protocol 1", "Protocol 2");
            //ReplaceStringInFile(file, "PermitRootLogin yes", "PermitRootLogin no");
            //ReplaceStringInFile(file, "ChallengeResponseAuthentication yes", "ChallengeResponseAuthentication no");
            //ReplaceStringInFile(file, "PasswordAuthentication yes", "PasswordAuthentication no");
            //ReplaceStringInFile(file, "UsePAM yes", "UsePAM no");
            //ReplaceStringInFile(file, "PubkeyAuthentication yes", "PubkeyAuthentication no");
            //ReplaceStringInFile(file, "PermitEmptyPasswords yes", "PermitEmptyPasswords no");

            //"echo \"install usb-storage /bin/true\" > /etc/modprobe.d/no-usb".Bash();
            //"sudo ufw deny 22".Bash();
            //"sudo ufw deny 2020/tcp".Bash();
            //"sudo apt-get update && sudo apt-get upgrade".Bash();

            foreach (Label mi in MyScreen.GetMenu().GetMenuItems())
            {
                mi.EditText("WE DID IT");
            }
        }

        static void Quit()
        {
            Console.BackgroundColor = StartColor;
            Console.Clear();
            Environment.Exit(0);
        }
    }

    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
